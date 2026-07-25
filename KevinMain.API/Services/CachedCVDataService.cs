using KevinMain.API.Models;

namespace KevinMain.API.Services;

/// <summary>
/// Cached implementation of CV data service that wraps any ICVDataService implementation.
/// Uses in-memory caching for fast access with 24-hour expiration.
/// This decorator pattern allows caching to work with any underlying data source (in-memory, database, API).
/// </summary>
public class CachedCVDataService : ICVDataService
{
    private readonly ICVDataService _innerService;
    private readonly ILogger<CachedCVDataService> _logger;
    private CVData? _cachedData;
    private DateTime _cacheExpiration = DateTime.MinValue;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(24);

    public CachedCVDataService(ICVDataService innerService, ILogger<CachedCVDataService> logger)
    {
        _innerService = innerService;
        _logger = logger;
    }

    public async Task<CVData> GetCVDataAsync()
    {
        if (_cachedData == null || DateTime.UtcNow > _cacheExpiration)
        {
            _logger.LogInformation("CV cache expired or empty, fetching from data source");
            try
            {
                _cachedData = await _innerService.GetCVDataAsync();
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
        }
        else
        {
            _logger.LogDebug("Returning CV data from in-memory cache");
        }

        return _cachedData;
    }
}
