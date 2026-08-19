using Azure;
using Azure.Data.Tables;
using KevinMain.API.Models;

namespace KevinMain.API.Services;

/// <summary>
/// Table Storage representation of a blog post.
/// All posts share a single partition ("blog") — the dataset is tiny —
/// and the RowKey is the normalized (lowercase) slug, giving efficient
/// point lookups by slug.
/// </summary>
public class BlogPostTableEntity : ITableEntity
{
    public const string BlogPartitionKey = "blog";

    public string PartitionKey { get; set; } = BlogPartitionKey;
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime? PublishedAt { get; set; }
    public bool IsPublished { get; set; }

    public static string NormalizeSlug(string slug) => slug.ToLowerInvariant();

    public static BlogPostTableEntity FromBlogPost(BlogPost post) => new()
    {
        PartitionKey = BlogPartitionKey,
        RowKey = NormalizeSlug(post.Slug),
        Id = post.Id,
        Title = post.Title,
        Slug = post.Slug,
        Content = post.Content,
        PublishedAt = post.PublishedAt?.ToUniversalTime(),
        IsPublished = post.IsPublished
    };

    public BlogPost ToBlogPost() => new()
    {
        Id = Id,
        Title = Title,
        Slug = Slug,
        Content = Content,
        PublishedAt = PublishedAt,
        IsPublished = IsPublished
    };
}
