using Azure.Data.Tables;
using KevinMain.API.Models;
using KevinMain.API.Services;

namespace KevinMain.API.Tests;

/// <summary>
/// Integration tests for TableStorageBlogPostRepository against a live Azurite emulator.
/// Opt-in via RUN_AZURITE_TESTS=true; when enabled, connection failures are test failures.
/// These tests assert behaviour (correct entity for an exact slug among mixed rows,
/// not-found as null, duplicate conflict, published filtering/sort) — the choice of
/// SDK point-lookup method is enforced by code review, not tests.
/// </summary>
public class TableStorageBlogPostRepositoryTests : IAsyncLifetime
{
    private const string AzuriteConnectionString = "UseDevelopmentStorage=true";

    private TableClient _tableClient = null!;
    private TableStorageBlogPostRepository _repository = null!;

    public async Task InitializeAsync()
    {
        // Unique table per test run so tests are isolated and repeatable.
        var tableName = $"blogtests{Guid.NewGuid():N}";
        _tableClient = new TableClient(AzuriteConnectionString, tableName);

        if (Enabled)
        {
            await _tableClient.CreateIfNotExistsAsync();
        }

        _repository = new TableStorageBlogPostRepository(_tableClient);
    }

    public async Task DisposeAsync()
    {
        if (Enabled)
        {
            await _tableClient.DeleteAsync();
        }
    }

    private static bool Enabled =>
        string.Equals(Environment.GetEnvironmentVariable("RUN_AZURITE_TESTS"), "true", StringComparison.OrdinalIgnoreCase);

    private static BlogPost CreatePost(string slug, bool published = true, DateTime? publishedAt = null) => new()
    {
        Id = Guid.NewGuid(),
        Title = $"Title for {slug}",
        Slug = slug,
        Content = $"Content for {slug}",
        PublishedAt = published ? (publishedAt ?? DateTime.UtcNow) : null,
        IsPublished = published
    };

    [AzuriteFact]
    public async Task GetBySlug_MixedRows_ReturnsCorrectEntity()
    {
        await _repository.AddAsync(CreatePost("first-post"));
        await _repository.AddAsync(CreatePost("second-post"));
        await _repository.AddAsync(CreatePost("third-post", published: false));

        var post = await _repository.GetBySlugAsync("second-post");

        Assert.NotNull(post);
        Assert.Equal("second-post", post!.Slug);
        Assert.Equal("Title for second-post", post.Title);
    }

    [AzuriteFact]
    public async Task GetBySlug_MixedCaseInput_ReturnsEntity()
    {
        await _repository.AddAsync(CreatePost("my-post"));

        var post = await _repository.GetBySlugAsync("MY-POST");

        Assert.NotNull(post);
        Assert.Equal("my-post", post!.Slug);
    }

    [AzuriteFact]
    public async Task GetBySlug_NotFound_ReturnsNullWithoutException()
    {
        var post = await _repository.GetBySlugAsync("does-not-exist");

        Assert.Null(post);
    }

    [AzuriteFact]
    public async Task Add_DuplicateSlug_ThrowsInvalidOperationException()
    {
        await _repository.AddAsync(CreatePost("dup-post"));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _repository.AddAsync(CreatePost("dup-post")));
    }

    [AzuriteFact]
    public async Task ListPublished_ExcludesUnpublished_AndSortsNewestFirst()
    {
        var older = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var newer = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc);

        await _repository.AddAsync(CreatePost("older-post", publishedAt: older));
        await _repository.AddAsync(CreatePost("newer-post", publishedAt: newer));
        await _repository.AddAsync(CreatePost("draft-post", published: false));

        var published = await _repository.ListPublishedAsync();

        Assert.Equal(2, published.Count);
        Assert.Equal("newer-post", published[0].Slug);
        Assert.Equal("older-post", published[1].Slug);
        Assert.DoesNotContain(published, p => p.Slug == "draft-post");
    }

    [AzuriteFact]
    public async Task RoundTrip_PreservesDraftWithNullPublishedAt()
    {
        await _repository.AddAsync(CreatePost("draft-roundtrip", published: false));

        var post = await _repository.GetBySlugAsync("draft-roundtrip");

        Assert.NotNull(post);
        Assert.Null(post!.PublishedAt);
        Assert.False(post.IsPublished);
    }
}
