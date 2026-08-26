using Microsoft.AspNetCore.Html;

namespace KevinMain.API.Models;

/// <summary>
/// View model for the server-rendered blog article page.
/// All string values are plain text (Razor encodes them on output);
/// <see cref="BodyHtml"/> is trusted HTML produced by Markdig with raw HTML disabled.
/// </summary>
public class BlogArticleViewModel
{
    public required string Title { get; init; }
    public required string PageTitle { get; init; }
    public required string Description { get; init; }
    public required string CanonicalUrl { get; init; }
    public required string SiteName { get; init; }
    public required string SiteBaseUrl { get; init; }
    public required string Author { get; init; }
    public DateTimeOffset? PublishedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }

    /// <summary>
    /// JSON-LD produced by <c>JsonSerializer.Serialize</c> (never string concatenation);
    /// the default encoder escapes HTML-sensitive characters so it is safe inside a script element.
    /// </summary>
    public required string JsonLd { get; init; }

    /// <summary>
    /// Article body rendered by Markdig with <c>DisableHtml()</c>.
    /// </summary>
    public required IHtmlContent BodyHtml { get; init; }
}
