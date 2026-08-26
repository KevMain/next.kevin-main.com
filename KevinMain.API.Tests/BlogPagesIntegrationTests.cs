using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using KevinMain.API.Models;
using KevinMain.API.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KevinMain.API.Tests;

/// <summary>
/// End-to-end tests for the server-rendered blog article pages (/blog/{slug})
/// and the legacy /post/{slug} 301 redirect, exercising the full HTTP pipeline
/// including the Razor view.
/// </summary>
public class BlogPagesIntegrationTests : IClassFixture<BlogPagesIntegrationTests.TestFactory>
{
    public class TestFactory : WebApplicationFactory<Program>
    {
        public InMemoryBlogPostRepository Repository { get; } = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Non-Development so the Azurite-only code paths (table creation) are skipped;
            // storage settings are valid-but-unused because the repository is replaced.
            builder.UseEnvironment("Staging");
            builder.UseSetting("BlogStorage:ServiceUri", "https://tests.table.core.windows.net");
            builder.UseSetting("BlogStorage:TableName", "blogposts");
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IBlogPostRepository>();
                services.AddSingleton<IBlogPostRepository>(Repository);
            });
        }
    }

    private const string BaseUrl = "https://www.kevin-main.com";

    private static readonly BlogPost PublishedPost = new()
    {
        Id = Guid.NewGuid(),
        Title = """C# & .NET: "What Changed?" <em>""",
        Slug = "csharp-dotnet-what-changed",
        MetaDescription = """Special <chars> & "quotes" in the description.""",
        Content = "## What Changed\n\nSome **bold** text.\n\n<script>alert('xss')</script>\n\n- item one\n- item two",
        PublishedAt = new DateTimeOffset(2026, 8, 26, 12, 0, 0, TimeSpan.FromHours(1)),
        UpdatedAt = new DateTimeOffset(2026, 8, 26, 14, 30, 0, TimeSpan.FromHours(1)),
        IsPublished = true
    };

    private static readonly BlogPost DraftPost = new()
    {
        Id = Guid.NewGuid(),
        Title = "Unpublished Draft",
        Slug = "unpublished-draft",
        Content = "Draft content",
        IsPublished = false
    };

    private readonly TestFactory _factory;

    public BlogPagesIntegrationTests(TestFactory factory)
    {
        _factory = factory;
        TryAdd(PublishedPost);
        TryAdd(DraftPost);
    }

    private void TryAdd(BlogPost post)
    {
        try { _factory.Repository.AddAsync(post).GetAwaiter().GetResult(); }
        catch (InvalidOperationException) { /* already added by a previous test */ }
    }

    private HttpClient CreateClient() =>
        _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    private async Task<string> GetPublishedArticleHtmlAsync()
    {
        var client = CreateClient();
        var response = await client.GetAsync($"/blog/{PublishedPost.Slug}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    [Fact]
    public async Task PublishedArticle_Returns200TextHtml()
    {
        var client = CreateClient();
        var response = await client.GetAsync($"/blog/{PublishedPost.Slug}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("text/html", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task PublishedArticle_ContainsEncodedH1AndBody()
    {
        var html = await GetPublishedArticleHtmlAsync();

        // Title special characters must be HTML-encoded, not raw
        Assert.Contains("<h1>C# &amp; .NET: &quot;What Changed?&quot; &lt;em&gt;</h1>", html);
        Assert.Contains("<strong>bold</strong>", html);
    }

    [Fact]
    public async Task PublishedArticle_HasCorrectTitleTag()
    {
        var html = await GetPublishedArticleHtmlAsync();

        Assert.Contains("<title>C# &amp; .NET: &quot;What Changed?&quot; &lt;em&gt; | Kevin Main</title>", html);
    }

    [Fact]
    public async Task PublishedArticle_DescriptionIsEncoded()
    {
        var html = await GetPublishedArticleHtmlAsync();

        Assert.Contains("""<meta name="description" content="Special &lt;chars&gt; &amp; &quot;quotes&quot; in the description.">""", html);
        // The raw unencoded description must not appear in an attribute
        Assert.DoesNotContain("""content="Special <chars>""", html);
    }

    [Fact]
    public async Task PublishedArticle_CanonicalEqualsBlogSlugUrl()
    {
        var html = await GetPublishedArticleHtmlAsync();

        Assert.Contains($"""<link rel="canonical" href="{BaseUrl}/blog/{PublishedPost.Slug}">""", html);
    }

    [Fact]
    public async Task PublishedArticle_HasValidBlogPostingJsonLd()
    {
        var html = await GetPublishedArticleHtmlAsync();

        var match = Regex.Match(html, """<script type="application/ld\+json">(.*?)</script>""", RegexOptions.Singleline);
        Assert.True(match.Success, "JSON-LD script block not found");

        using var doc = JsonDocument.Parse(match.Groups[1].Value);
        var root = doc.RootElement;

        Assert.Equal("https://schema.org", root.GetProperty("@context").GetString());
        Assert.Equal("BlogPosting", root.GetProperty("@type").GetString());
        Assert.Equal(PublishedPost.Title, root.GetProperty("headline").GetString());
        Assert.Equal(PublishedPost.MetaDescription, root.GetProperty("description").GetString());
        Assert.Equal($"{BaseUrl}/blog/{PublishedPost.Slug}", root.GetProperty("url").GetString());
        Assert.Equal(PublishedPost.PublishedAt!.Value, DateTimeOffset.Parse(root.GetProperty("datePublished").GetString()!));
        Assert.Equal(PublishedPost.UpdatedAt!.Value, DateTimeOffset.Parse(root.GetProperty("dateModified").GetString()!));
        Assert.Equal("Person", root.GetProperty("author").GetProperty("@type").GetString());
        Assert.Equal("Kevin Main", root.GetProperty("author").GetProperty("name").GetString());
        Assert.Equal($"{BaseUrl}/", root.GetProperty("author").GetProperty("url").GetString());
        Assert.Equal("Kevin Main", root.GetProperty("publisher").GetProperty("name").GetString());
        Assert.Equal($"{BaseUrl}/blog/{PublishedPost.Slug}", root.GetProperty("mainEntityOfPage").GetProperty("@id").GetString());
    }

    [Fact]
    public async Task UnpublishedArticle_Returns404()
    {
        var client = CreateClient();
        var response = await client.GetAsync($"/blog/{DraftPost.Slug}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UnknownSlug_Returns404()
    {
        var client = CreateClient();
        var response = await client.GetAsync("/blog/does-not-exist");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task LegacyPostUrl_Returns301ToBlogUrl()
    {
        var client = CreateClient();
        var response = await client.GetAsync($"/post/{PublishedPost.Slug}");

        Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
        Assert.Equal($"/blog/{PublishedPost.Slug}", response.Headers.Location?.ToString());
    }

    [Fact]
    public async Task MarkdownIsConverted()
    {
        var html = await GetPublishedArticleHtmlAsync();

        Assert.Contains("<h2", html);          // ## heading
        Assert.Contains("What Changed</h2>", html);
        Assert.Contains("<li>item one</li>", html);
        Assert.Contains("<li>item two</li>", html);
    }

    [Fact]
    public async Task EmbeddedRawHtml_DoesNotRender()
    {
        var html = await GetPublishedArticleHtmlAsync();

        Assert.DoesNotContain("<script>alert", html);
        Assert.Contains("&lt;script&gt;alert('xss')&lt;/script&gt;", html);
    }

    [Fact]
    public async Task SpecialCharacters_DoNotBreakJsonLd()
    {
        var html = await GetPublishedArticleHtmlAsync();

        var match = Regex.Match(html, """<script type="application/ld\+json">(.*?)</script>""", RegexOptions.Singleline);
        Assert.True(match.Success);

        // Parsing succeeds and the headline round-trips exactly, including & " < >
        using var doc = JsonDocument.Parse(match.Groups[1].Value);
        Assert.Equal("""C# & .NET: "What Changed?" <em>""", doc.RootElement.GetProperty("headline").GetString());

        // No raw </script> or unescaped angle brackets can terminate the script element early
        Assert.DoesNotContain("<em>", match.Groups[1].Value);
    }
}
