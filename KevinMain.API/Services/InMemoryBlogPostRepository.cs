using System.Collections.Concurrent;
using KevinMain.API.Models;

namespace KevinMain.API.Services;

/// <summary>
/// In-memory blog post repository backed by a ConcurrentDictionary keyed by slug.
/// Registered as a singleton so posts persist for the lifetime of the application.
/// </summary>
public class InMemoryBlogPostRepository : IBlogPostRepository
{
    private readonly ConcurrentDictionary<string, BlogPost> _posts = new(StringComparer.OrdinalIgnoreCase);

    public Task<BlogPost> AddAsync(BlogPost post)
    {
        ArgumentNullException.ThrowIfNull(post);
        ArgumentException.ThrowIfNullOrWhiteSpace(post.Slug);

        if (!_posts.TryAdd(post.Slug, post))
        {
            throw new InvalidOperationException($"A blog post with slug '{post.Slug}' already exists.");
        }

        return Task.FromResult(post);
    }

    public Task<BlogPost?> GetBySlugAsync(string slug)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        _posts.TryGetValue(slug, out var post);
        return Task.FromResult(post);
    }

    public Task<IReadOnlyList<BlogPost>> ListPublishedAsync()
    {
        IReadOnlyList<BlogPost> published = _posts.Values
            .Where(p => p.IsPublished)
            .OrderByDescending(p => p.PublishedAt)
            .ToList();

        return Task.FromResult(published);
    }
}
