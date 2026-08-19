using KevinMain.API.Models;
using Microsoft.Extensions.Caching.Hybrid;

namespace KevinMain.API.Services;

/// <summary>
/// Caching decorator for <see cref="IBlogPostRepository"/> using HybridCache.
/// Posts change infrequently, so found posts and the published list are cached
/// generously; misses are negative-cached briefly to absorb crawler/bot traffic
/// on nonexistent URLs. All mutations invalidate the affected post key(s) and
/// the published list in a single multi-key remove.
/// </summary>
public class CachedBlogPostRepository : IBlogPostRepository
{
    private const string PublishedListKey = "blog:list:published";

    private readonly IBlogPostRepository _inner;
    private readonly HybridCache _cache;
    private readonly BlogCacheSettings _settings;

    public CachedBlogPostRepository(IBlogPostRepository inner, HybridCache cache, BlogCacheSettings settings)
    {
        _inner = inner;
        _cache = cache;
        _settings = settings;
    }

    // The only place cache keys are constructed. Slugs are normalized so that
    // case-insensitive lookups converge on the same cache entry.
    private static string PostKey(string slug) => $"blog:slug:{slug.ToLowerInvariant()}";

    public async Task<BlogPost> AddAsync(BlogPost post)
    {
        var created = await _inner.AddAsync(post);

        ArgumentException.ThrowIfNullOrWhiteSpace(created.Slug);

        // Write succeeded → invalidate the individual post (clears any negative
        // entry for this slug) and the published list.
        await InvalidateAsync([created.Slug], CancellationToken.None);

        return created;
    }

    public async Task<BlogPost?> GetBySlugAsync(string slug)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        var key = PostKey(slug);

        var post = await _cache.GetOrCreateAsync<BlogPost?>(
            key,
            async _ => await _inner.GetBySlugAsync(slug),
            new HybridCacheEntryOptions { Expiration = _settings.PostDuration });

        if (post is null)
        {
            // Negative caching: keep "not found" for a shorter duration than a
            // real post, so bot/crawler hits on random slugs don't reach storage
            // repeatedly but a newly-published post appears promptly.
            await _cache.SetAsync<BlogPost?>(
                key,
                null,
                new HybridCacheEntryOptions { Expiration = _settings.NotFoundDuration });
        }

        return post;
    }

    public async Task<IReadOnlyList<BlogPost>> ListPublishedAsync()
    {
        return await _cache.GetOrCreateAsync<IReadOnlyList<BlogPost>>(
            PublishedListKey,
            async _ => await _inner.ListPublishedAsync(),
            new HybridCacheEntryOptions { Expiration = _settings.PublishedListDuration });
    }

    private async Task InvalidateAsync(IEnumerable<string> slugs, CancellationToken cancellationToken)
    {
        var keys = slugs
            .Select(PostKey)
            .Append(PublishedListKey)
            .Distinct()
            .ToArray();

        await _cache.RemoveAsync(keys, cancellationToken);
    }
}
