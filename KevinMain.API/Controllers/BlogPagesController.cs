using System.Text.Json;
using System.Text.Json.Serialization;
using KevinMain.API.Models;
using KevinMain.API.Services;
using Markdig;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace KevinMain.API.Controllers;

/// <summary>
/// Serves fully server-rendered, crawlable HTML pages for blog articles at /blog/{slug}.
/// The document template lives in Views/BlogPages/Article.cshtml (Razor auto-encodes
/// all metadata); this controller owns retrieval and HTTP behaviour only.
/// </summary>
[Route("")]
public class BlogPagesController : Controller
{
    private static readonly MarkdownPipeline MarkdownPipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .DisableHtml()
        .Build();

    private static readonly JsonSerializerOptions JsonLdOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly IBlogPostRepository _repository;
    private readonly SiteSettings _site;

    public BlogPagesController(IBlogPostRepository repository, SiteSettings site)
    {
        _repository = repository;
        _site = site;
    }

    /// <summary>
    /// Permanent redirect from the legacy /post/{slug} URLs to /blog/{slug}.
    /// </summary>
    [HttpGet("post/{slug}")]
    public IActionResult LegacyPostRedirect(string slug) =>
        RedirectPermanent($"/blog/{Uri.EscapeDataString(slug)}");

    /// <summary>
    /// Server-rendered blog article page.
    /// </summary>
    [HttpGet("blog/{slug}")]
    public async Task<IActionResult> ArticlePage(string slug)
    {
        var post = await _repository.GetBySlugAsync(slug);
        if (post is null || !post.IsPublished)
        {
            return NotFound();
        }

        return View("Article", BuildViewModel(post));
    }

    private BlogArticleViewModel BuildViewModel(BlogPost post)
    {
        var baseUrl = _site.BaseUrl.TrimEnd('/');
        var canonicalUrl = $"{baseUrl}/blog/{Uri.EscapeDataString(post.Slug)}";
        var description = string.IsNullOrWhiteSpace(post.MetaDescription)
            ? BuildExcerpt(post.Content)
            : post.MetaDescription;

        var jsonLd = JsonSerializer.Serialize(new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "BlogPosting",
            ["headline"] = post.Title,
            ["description"] = description,
            ["url"] = canonicalUrl,
            ["datePublished"] = post.PublishedAt?.ToString("O"),
            ["dateModified"] = (post.UpdatedAt ?? post.PublishedAt)?.ToString("O"),
            ["author"] = new Dictionary<string, object?>
            {
                ["@type"] = "Person",
                ["name"] = _site.DefaultAuthor,
                ["url"] = $"{baseUrl}/"
            },
            ["publisher"] = new Dictionary<string, object?>
            {
                ["@type"] = "Person",
                ["name"] = _site.SiteName
            },
            ["mainEntityOfPage"] = new Dictionary<string, object?>
            {
                ["@type"] = "WebPage",
                ["@id"] = canonicalUrl
            }
        }, JsonLdOptions);

        return new BlogArticleViewModel
        {
            Title = post.Title,
            PageTitle = $"{post.Title} | {_site.SiteName}",
            Description = description,
            CanonicalUrl = canonicalUrl,
            SiteName = _site.SiteName,
            SiteBaseUrl = baseUrl,
            Author = _site.DefaultAuthor,
            PublishedAt = post.PublishedAt,
            UpdatedAt = post.UpdatedAt,
            JsonLd = jsonLd,
            BodyHtml = new HtmlString(Markdown.ToHtml(post.Content, MarkdownPipeline))
        };
    }

    /// <summary>
    /// Builds a plain-text excerpt from markdown content for use as a fallback meta description.
    /// </summary>
    private static string BuildExcerpt(string markdown, int maxLength = 160)
    {
        var plain = Markdown.ToPlainText(markdown, MarkdownPipeline)
            .Replace('\n', ' ')
            .Replace('\r', ' ')
            .Trim();
        while (plain.Contains("  "))
        {
            plain = plain.Replace("  ", " ");
        }

        if (plain.Length <= maxLength)
        {
            return plain;
        }

        var cut = plain.LastIndexOf(' ', maxLength);
        return string.Concat(plain.AsSpan(0, cut > 0 ? cut : maxLength), "…");
    }
}
