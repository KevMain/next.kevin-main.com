using KevinMain.API.Models;
using System.Text.Json;

namespace KevinMain.API.Services;

/// <summary>
/// Cached implementation of CV data service that wraps any ICVDataService implementation.
/// Uses two-tier caching: in-memory for fast access, file-based for persistence across restarts.
/// This decorator pattern allows caching to work with any underlying data source (in-memory, database, API).
/// </summary>
public class CachedCVDataService : ICVDataService
{
    private readonly ICVDataService _innerService;
    private readonly ILogger<CachedCVDataService> _logger;
    private CVData? _cachedData;
    private DateTime _cacheExpiration = DateTime.MinValue;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(24);
    private readonly string _cacheFilePath = Path.Combine(Path.GetTempPath(), "cv_data_cache.json");
    private readonly string _cacheMetaFilePath = Path.Combine(Path.GetTempPath(), "cv_cache_meta.json");

    public CachedCVDataService(ICVDataService innerService, ILogger<CachedCVDataService> logger)
    {
        _innerService = innerService;
        _logger = logger;
    }

    public async Task<CVData> GetCVDataAsync()
    {
        // Try to load from file cache first if memory cache is empty
        if (_cachedData == null)
        {
            await LoadFromFileCache();
        }

        if (_cachedData == null || DateTime.UtcNow > _cacheExpiration)
        {
            _logger.LogInformation("CV cache expired or empty, fetching from data source");
            try
            {
                _cachedData = await _innerService.GetCVDataAsync();
                _cacheExpiration = DateTime.UtcNow.Add(_cacheDuration);

                // Save to file cache
                await SaveToFileCache();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch CV data from source");

                // Try to use existing file cache even if expired
                if (_cachedData == null)
                {
                    await LoadFromFileCache();
                }

                // If still null, rethrow - CV data is critical
                if (_cachedData == null)
                {
                    throw;
                }

                _logger.LogWarning("Using expired cache due to data source failure");
            }
        }

        return _cachedData;
    }

    private async Task LoadFromFileCache()
    {
        try
        {
            if (File.Exists(_cacheFilePath) && File.Exists(_cacheMetaFilePath))
            {
                var metaJson = await File.ReadAllTextAsync(_cacheMetaFilePath);
                var meta = JsonSerializer.Deserialize<CacheMeta>(metaJson);

                if (meta != null && meta.Expiration > DateTime.UtcNow)
                {
                    var json = await File.ReadAllTextAsync(_cacheFilePath);
                    _cachedData = JsonSerializer.Deserialize<CVData>(json);
                    _cacheExpiration = meta.Expiration;
                    _logger.LogInformation("Loaded CV data from file cache, expires at {Expiration}", _cacheExpiration);
                }
                else
                {
                    _logger.LogInformation("CV file cache expired");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load CV data from file cache");
        }
    }

    private async Task SaveToFileCache()
    {
        try
        {
            if (_cachedData != null)
            {
                var json = JsonSerializer.Serialize(_cachedData);
                await File.WriteAllTextAsync(_cacheFilePath, json);

                var meta = new CacheMeta { Expiration = _cacheExpiration };
                var metaJson = JsonSerializer.Serialize(meta);
                await File.WriteAllTextAsync(_cacheMetaFilePath, metaJson);

                _logger.LogInformation("Saved CV data to file cache");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to save CV data to file cache");
        }
    }

    private class CacheMeta
    {
        public DateTime Expiration { get; set; }
    }
}
