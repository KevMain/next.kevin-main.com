namespace KevinMain.API.Models;

/// <summary>
/// Cache durations for blog post data. Named after behaviour, not implementation:
/// posts change infrequently so generous durations are appropriate, while
/// not-found results are cached briefly to absorb crawler/bot traffic.
/// </summary>
public class BlogCacheSettings
{
    /// <summary>How long a found blog post is cached.</summary>
    public TimeSpan PostDuration { get; set; } = TimeSpan.FromHours(1);

    /// <summary>How long the published post list is cached.</summary>
    public TimeSpan PublishedListDuration { get; set; } = TimeSpan.FromMinutes(30);

    /// <summary>How long a "post not found" result is cached (negative caching).</summary>
    public TimeSpan NotFoundDuration { get; set; } = TimeSpan.FromMinutes(5);
}
