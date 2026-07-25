namespace KevinMain.API.Models;

/// <summary>
/// Configuration settings for caching behavior in the application
/// </summary>
public class CachingSettings
{
    /// <summary>
    /// Number indicating how long CV data should be cached in hours.
    /// Default is 24 hours if not specified in configuration.
    /// </summary>
    public int CVCacheDurationHours { get; set; } = 24;
}
