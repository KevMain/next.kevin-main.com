using KevinMain.API.Extensions;
using KevinMain.API.Models;

namespace KevinMain.API.Services;

/// <summary>
/// Cached implementation of CV data service that wraps any ICVDataService implementation.
/// Uses in-memory caching for fast access with configurable expiration.
/// This decorator pattern allows caching to work with any underlying data source (in-memory, database, API).
/// Thread-safe for concurrent access using SemaphoreSlim.
/// </summary>
public class CachedCVDataService : ICVDataService, IDisposable
{
    private readonly ICVDataService _innerService;
    private readonly ILogger<CachedCVDataService> _logger;
    private readonly SemaphoreSlim _cacheLock = new(1, 1);
    private CVData? _cachedData;
    private DateTime _cacheExpiration = DateTime.MinValue;
    private readonly TimeSpan _cacheDuration;

    public CachedCVDataService(ICVDataService innerService, ILogger<CachedCVDataService> logger, CachingSettings cachingSettings)
    {
        _innerService = innerService;
        _logger = logger;
        _cacheDuration = TimeSpan.FromHours(cachingSettings.CVCacheDurationHours);
        _logger.LogInformation("CachedCVDataService initialized with cache duration of {DurationHours} hours", cachingSettings.CVCacheDurationHours);
    }

    public async Task<CVData> GetCVDataAsync()
    {
        // Fast path: cache is valid and available
        if (_cachedData != null && DateTime.UtcNow <= _cacheExpiration)
        {
            _logger.LogDebug("Returning CV data from in-memory cache");
            return _cachedData;
        }

        // Slow path: need to refresh cache (thread-safe)
        await _cacheLock.WaitAsync().ConfigureAwait(false);
        try
        {
            // Double-check after acquiring lock (another thread might have refreshed)
            if (_cachedData != null && DateTime.UtcNow <= _cacheExpiration)
            {
                _logger.LogDebug("Cache was refreshed by another thread, returning cached data");
                return _cachedData;
            }

            _logger.LogInformation("CV cache expired or empty, fetching from data source");
            try
            {
                _cachedData = await _innerService.GetCVDataAsync().ConfigureAwait(false);
                _cacheExpiration = DateTime.UtcNow.Add(_cacheDuration);
                _logger.LogInformation("CV data cached successfully, expires at {Expiration}", _cacheExpiration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch CV data from source");

                // If we have stale cache, use it
                if (_cachedData != null)
                {
                    _logger.LogWarning("Using expired cache due to data source failure");
                }
                else
                {
                    // No cache available, rethrow - CV data is critical
                    throw;
                }
            }

            return _cachedData!; // Guaranteed non-null at this point
        }
        finally
        {
            _cacheLock.Release();
        }
    }

    public async Task<PersonalInfo> GetPersonalInfoAsync()
    {
        var cvData = await GetCVDataAsync();
        return cvData.PersonalInfo;
    }

    public async Task<ProfileData> GetProfileAsync()
    {
        var cvData = await GetCVDataAsync();
        return cvData.ToProfileData();
    }

    public async Task<List<WorkExperience>> GetWorkExperienceAsync()
    {
        var cvData = await GetCVDataAsync();
        return cvData.WorkExperience;
    }

    public async Task<Education> GetEducationAsync()
    {
        var cvData = await GetCVDataAsync();
        return cvData.Education;
    }

    public async Task<string> GetLeisureActivitiesAsync()
    {
        var cvData = await GetCVDataAsync();
        return cvData.LeisureActivities;
    }

    public void Dispose()
    {
        _cacheLock?.Dispose();
    }
}
