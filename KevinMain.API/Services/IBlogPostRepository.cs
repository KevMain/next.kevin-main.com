using KevinMain.API.Models;

namespace KevinMain.API.Services;

/// <summary>
/// Repository for blog posts
/// </summary>
public interface IBlogPostRepository
{
    /// <summary>
    /// Adds a new blog post. Throws InvalidOperationException if the slug already exists.
    /// </summary>
    Task<BlogPost> AddAsync(BlogPost post);

    /// <summary>
    /// Gets a blog post by its slug, or null if not found.
    /// </summary>
    Task<BlogPost?> GetBySlugAsync(string slug);

    /// <summary>
    /// Lists all published blog posts, newest first.
    /// </summary>
    Task<IReadOnlyList<BlogPost>> ListPublishedAsync();
}
