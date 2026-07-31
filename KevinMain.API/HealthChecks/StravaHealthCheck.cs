using Microsoft.Extensions.Diagnostics.HealthChecks;
using KevinMain.API.Models;

namespace KevinMain.API.HealthChecks;

/// <summary>
/// Health check for Strava API connectivity
/// </summary>
public class StravaHealthCheck : IHealthCheck
{
    private readonly StravaSettings _settings;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<StravaHealthCheck> _logger;

    public StravaHealthCheck(
        StravaSettings settings,
        IHttpClientFactory httpClientFactory,
        ILogger<StravaHealthCheck> logger)
    {
        _settings = settings;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        // If Strava is disabled, it's healthy (not required)
        if (!_settings.Enabled)
        {
            return HealthCheckResult.Healthy("Strava integration is disabled");
        }

        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(5);

            // Check if Strava API is reachable (simple connectivity test)
            var response = await httpClient.GetAsync(
                "https://www.strava.com/api/v3/athlete",
                cancellationToken);

            // We expect 401 Unauthorized (no token), but that means API is reachable
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return HealthCheckResult.Healthy("Strava API is reachable");
            }

            // Any other response means something might be wrong
            return HealthCheckResult.Degraded(
                $"Strava API returned unexpected status: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Strava API health check failed: connectivity issue");
            return HealthCheckResult.Unhealthy(
                "Strava API is unreachable",
                ex);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "Strava API health check failed: timeout");
            return HealthCheckResult.Degraded(
                "Strava API health check timed out",
                ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Strava API health check failed unexpectedly");
            return HealthCheckResult.Unhealthy(
                "Strava API health check failed",
                ex);
        }
    }
}
