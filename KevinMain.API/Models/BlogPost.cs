using System.ComponentModel.DataAnnotations;

namespace KevinMain.API.Models;

/// <summary>
/// Represents a blog post
/// </summary>
public class BlogPost
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// When the post was published. Null for drafts (IsPublished = false).
    /// </summary>
    public DateTime? PublishedAt { get; set; }
    public bool IsPublished { get; set; }
}

/// <summary>
/// Request payload for creating a blog post
/// </summary>
public class CreateBlogPostRequest
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    [RegularExpression("^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must be lowercase letters, numbers and hyphens.")]
    public string Slug { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    public bool IsPublished { get; set; }
}
