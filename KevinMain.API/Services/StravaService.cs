using KevinMain.API.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace KevinMain.API.Services;

public interface IStravaService
{
    Task<List<RunningActivity>> GetActivitiesAsync(int count = 30);
}

public class StravaService : IStravaService
{
    private readonly HttpClient _httpClient;
    private readonly StravaSettings _settings;
    private readonly ILogger<StravaService> _logger;
    private string? _accessToken;
    private DateTime _tokenExpiresAt = DateTime.MinValue;

    public StravaService(HttpClient httpClient, StravaSettings settings, ILogger<StravaService> logger)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;
        _httpClient.BaseAddress = new Uri("https://www.strava.com/api/v3/");
    }

    private async Task<string> GetAccessTokenAsync()
    {
        if (_accessToken != null && DateTime.UtcNow < _tokenExpiresAt)
        {
            _logger.LogInformation("Using cached access token");
            return _accessToken;
        }

        try
        {
            _logger.LogInformation("Refreshing Strava access token...");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://www.strava.com/oauth/token");
            var formData = new Dictionary<string, string>
            {
                { "client_id", _settings.ClientId },
                { "client_secret", _settings.ClientSecret },
                { "refresh_token", _settings.RefreshToken },
                { "grant_type", "refresh_token" }
            };

            request.Content = new FormUrlEncodedContent(formData);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Token refresh failed with status {StatusCode}: {Error}", 
                    response.StatusCode, errorContent);
                throw new Exception($"Token refresh failed: {response.StatusCode} - {errorContent}");
            }

            var tokenResponse = await response.Content.ReadFromJsonAsync<StravaTokenResponse>();
            if (tokenResponse == null)
            {
                throw new Exception("Failed to parse token response");
            }

            _accessToken = tokenResponse.access_token;
            _tokenExpiresAt = DateTimeOffset.FromUnixTimeSeconds(tokenResponse.expires_at).UtcDateTime;

            _logger.LogInformation("Successfully refreshed Strava access token. Expires at: {ExpiresAt}", 
                _tokenExpiresAt);
            return _accessToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to refresh Strava access token");
            throw;
        }
    }

    public async Task<List<RunningActivity>> GetActivitiesAsync(int count = 30)
    {
        if (!_settings.Enabled)
        {
            _logger.LogWarning("Strava integration is not enabled");
            return new List<RunningActivity>();
        }

        // Validate settings
        if (string.IsNullOrEmpty(_settings.ClientId) || 
            string.IsNullOrEmpty(_settings.ClientSecret) || 
            string.IsNullOrEmpty(_settings.RefreshToken))
        {
            _logger.LogError("Strava settings are incomplete. Please check ClientId, ClientSecret, and RefreshToken");
            throw new InvalidOperationException("Strava settings are not properly configured");
        }

        try
        {
            var token = await GetAccessTokenAsync();

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            _logger.LogInformation("Fetching {Count} activities from Strava...", count);
            var response = await _httpClient.GetAsync($"athlete/activities?per_page={count}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to fetch activities. Status: {StatusCode}, Error: {Error}", 
                    response.StatusCode, errorContent);
                throw new Exception($"Failed to fetch activities: {response.StatusCode} - {errorContent}");
            }

            var stravaActivities = await response.Content.ReadFromJsonAsync<List<StravaActivity>>();
            if (stravaActivities == null)
            {
                return new List<RunningActivity>();
            }

            var runningActivities = stravaActivities
                .Where(a => a.type == "Run")
                .Select((a, index) => ConvertToRunningActivity(a, index + 1))
                .ToList();

            _logger.LogInformation("Successfully fetched {Count} running activities from Strava", runningActivities.Count);
            return runningActivities;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch activities from Strava");
            throw;
        }
    }

    private RunningActivity ConvertToRunningActivity(StravaActivity strava, int id)
    {
        var distanceKm = (decimal)strava.distance / 1000m; // Convert meters to km
        var duration = TimeSpan.FromSeconds(strava.moving_time);
        var averagePace = duration.TotalMinutes / (double)distanceKm;

        return new RunningActivity
        {
            Id = id,
            Date = strava.start_date,
            Title = strava.name,
            Distance = distanceKm,
            Duration = duration,
            Location = null, // Strava doesn't provide location name in basic API
            Description = strava.description,
            ImageUrl = null,
            AveragePace = (decimal)averagePace,
            Elevation = (int?)strava.total_elevation_gain,
            InstagramPostUrl = null,
            StravaUrl = $"https://www.strava.com/activities/{strava.id}",
            Tags = new List<string> { strava.type.ToLower() }
        };
    }
}
