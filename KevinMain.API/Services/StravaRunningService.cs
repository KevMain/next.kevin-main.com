using KevinMain.API.Models;
using System.Text.Json;

namespace KevinMain.API.Services;

public class StravaRunningService : IRunningService
{
    private readonly IStravaService _stravaService;
    private readonly ILogger<StravaRunningService> _logger;
    private readonly IWebHostEnvironment _environment;
    private List<RunningActivity>? _cachedActivities;
    private DateTime _cacheExpiration = DateTime.MinValue;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(24);
    private readonly string _cacheFilePath = Path.Combine(Path.GetTempPath(), "strava_activities_cache.json");
    private readonly string _cacheMetaFilePath = Path.Combine(Path.GetTempPath(), "strava_cache_meta.json");

    public StravaRunningService(IStravaService stravaService, ILogger<StravaRunningService> logger, IWebHostEnvironment environment)
    {
        _stravaService = stravaService;
        _logger = logger;
        _environment = environment;
    }

    private async Task<List<RunningActivity>> GetCachedActivitiesAsync()
    {
        // Try to load from file cache first if memory cache is empty
        if (_cachedActivities == null)
        {
            await LoadFromFileCache();
        }

        if (_cachedActivities == null || DateTime.UtcNow > _cacheExpiration)
        {
            _logger.LogInformation("Cache expired or empty, fetching from Strava");
            try
            {
                _cachedActivities = await _stravaService.GetActivitiesAsync(50);
                _cacheExpiration = DateTime.UtcNow.Add(_cacheDuration);

                // Save to file cache
                await SaveToFileCache();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch from Strava");

                // Try to use existing file cache even if expired
                if (_cachedActivities == null)
                {
                    await LoadFromFileCache();
                }

                // If still null, return empty list
                if (_cachedActivities == null)
                {
                    _cachedActivities = new List<RunningActivity>();
                }
            }
        }

        return _cachedActivities;
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
                    _cachedActivities = JsonSerializer.Deserialize<List<RunningActivity>>(json);
                    _cacheExpiration = meta.Expiration;
                    _logger.LogInformation("Loaded {Count} activities from file cache, expires at {Expiration}", 
                        _cachedActivities?.Count ?? 0, _cacheExpiration);
                }
                else
                {
                    _logger.LogInformation("File cache expired");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load from file cache");
        }
    }

    private async Task SaveToFileCache()
    {
        try
        {
            if (_cachedActivities != null)
            {
                var json = JsonSerializer.Serialize(_cachedActivities);
                await File.WriteAllTextAsync(_cacheFilePath, json);

                var meta = new CacheMeta { Expiration = _cacheExpiration };
                var metaJson = JsonSerializer.Serialize(meta);
                await File.WriteAllTextAsync(_cacheMetaFilePath, metaJson);

                _logger.LogInformation("Saved {Count} activities to file cache", _cachedActivities.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to save to file cache");
        }
    }

    private class CacheMeta
    {
        public DateTime Expiration { get; set; }
    }

    public async Task<IEnumerable<RunningActivity>> GetAllActivitiesAsync()
    {
        var activities = await GetCachedActivitiesAsync();
        return activities.OrderByDescending(a => a.Date);
    }

    public async Task<RunningActivity?> GetActivityByIdAsync(int id)
    {
        var activities = await GetCachedActivitiesAsync();
        return activities.FirstOrDefault(a => a.Id == id);
    }

    public Task<PersonalBests> GetPersonalBestsAsync()
    {
        return Task.FromResult(GetHardcodedPBs());
    }

    private PersonalBests GetHardcodedPBs()
    {
        return new PersonalBests
        {
            Fastest5K = new RunBest
            {
                Title = "5K Personal Best",
                Date = new DateTime(2024, 3, 15),
                Distance = 5.0m,
                Duration = TimeSpan.FromMinutes(18).Add(TimeSpan.FromSeconds(33)),
                Pace = 3.71m,
                Elevation = 45
            },
            Fastest10K = new RunBest
            {
                Title = "10K Personal Best",
                Date = new DateTime(2023, 9, 10),
                Distance = 10.0m,
                Duration = TimeSpan.FromMinutes(39).Add(TimeSpan.FromSeconds(14)),
                Pace = 3.92m,
                Elevation = 85
            },
            FastestHalfMarathon = new RunBest
            {
                Title = "Half Marathon Personal Best",
                Date = new DateTime(2023, 5, 21),
                Distance = 21.1m,
                Duration = TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(27)).Add(TimeSpan.FromSeconds(55)),
                Pace = 4.17m,
                Elevation = 180
            },
            FastestMarathon = new RunBest
            {
                Title = "Marathon Personal Best",
                Date = new DateTime(2022, 10, 2),
                Distance = 42.2m,
                Duration = TimeSpan.FromHours(3).Add(TimeSpan.FromMinutes(39)).Add(TimeSpan.FromSeconds(12)),
                Pace = 5.19m,
                Elevation = 320
            }
        };
    }

    public async Task<IEnumerable<RunningActivity>> GetRecentActivitiesAsync(int count = 5)
    {
        var activities = await GetCachedActivitiesAsync();
        return activities.OrderByDescending(a => a.Date).Take(count);
    }

    public Task<IEnumerable<string>> GetRecentImagesAsync(int count = 6)
    {
        var imagesPath = Path.Combine(_environment.WebRootPath, "images", "running");

        // Ensure directory exists
        if (!Directory.Exists(imagesPath))
        {
            Directory.CreateDirectory(imagesPath);
        }

        // Get all image files from the directory
        var supportedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var imageFiles = Directory.GetFiles(imagesPath)
            .Where(f => supportedExtensions.Contains(Path.GetExtension(f).ToLower()))
            .OrderByDescending(f => new FileInfo(f).CreationTime)
            .Take(count)
            .Select(f => $"/images/running/{Path.GetFileName(f)}")
            .ToList();

        // If no images found, return placeholder images
        if (!imageFiles.Any())
        {
            imageFiles = new List<string>
            {
                "https://images.unsplash.com/photo-1552674605-db6ffd4facb5?w=400&h=400&fit=crop",
                "https://images.unsplash.com/photo-1571008887538-b36bb32f4571?w=400&h=400&fit=crop",
                "https://images.unsplash.com/photo-1483721310020-03333e577078?w=400&h=400&fit=crop",
                "https://images.unsplash.com/photo-1476480862126-209bfaa8edc8?w=400&h=400&fit=crop",
                "https://images.unsplash.com/photo-1523633589114-88eaf4b4f1a8?w=400&h=400&fit=crop",
                "https://images.unsplash.com/photo-1519315901367-f34ff9154487?w=400&h=400&fit=crop"
            };
        }

        return Task.FromResult(imageFiles.Take(count).AsEnumerable());
    }
}
