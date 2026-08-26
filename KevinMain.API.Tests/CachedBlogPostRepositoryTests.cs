using KevinMain.API.Models;
using KevinMain.API.Services;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

namespace KevinMain.API.Tests;

/// <summary>
/// Counting fake used to assert how many calls reach the inner repository
/// through the caching decorator.
/// </summary>
internal class CountingBlogPostRepository : IBlogPostRepository
{
    private readonly InMemoryBlogPostRepository _store = new();

    public int GetBySlugCalls { get; private set; }
    public int ListPublishedCalls { get; private set; }

    public Task<BlogPost> AddAsync(BlogPost post) => _store.AddAsync(post);

    public Task<BlogPost?> GetBySlugAsync(string slug)
    {
        GetBySlugCalls++;
        return _store.GetBySlugAsync(slug);
    }

    public Task<IReadOnlyList<BlogPost>> ListPublishedAsync()
    {
        ListPublishedCalls++;
        return _store.ListPublishedAsync();
    }
}

public class CachedBlogPostRepositoryTests
{
    private static HybridCache CreateCache()
    {
        var services = new ServiceCollection();
        services.AddHybridCache();
        return services.BuildServiceProvider().GetRequiredService<HybridCache>();
    }

    private static (CachedBlogPostRepository Cached, CountingBlogPostRepository Inner) CreateSut()
    {
        var inner = new CountingBlogPostRepository();
        var cached = new CachedBlogPostRepository(inner, CreateCache(), new BlogCacheSettings());
        return (cached, inner);
    }

    private static BlogPost CreatePost(string slug = "my-post", bool published = true) => new()
    {
        Id = Guid.NewGuid(),
        Title = "Title",
        Slug = slug,
        Content = "Content",
        PublishedAt = published ? DateTime.UtcNow : null,
        IsPublished = published
    };

    [Fact]
    public async Task GetBySlug_SecondCall_DoesNotHitInnerRepository()
    {
        var (cached, inner) = CreateSut();
        await inner.AddAsync(CreatePost());

        var first = await cached.GetBySlugAsync("my-post");
        var second = await cached.GetBySlugAsync("my-post");

        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.Equal(1, inner.GetBySlugCalls);
    }

    [Fact]
    public async Task GetBySlug_CaseInsensitiveLookups_UseSameCacheEntry()
    {
        var (cached, inner) = CreateSut();
        await inner.AddAsync(CreatePost());

        var lower = await cached.GetBySlugAsync("my-post");
        var upper = await cached.GetBySlugAsync("MY-POST");

        Assert.NotNull(lower);
        Assert.NotNull(upper);
        Assert.Equal(lower!.Id, upper!.Id);
        Assert.Equal(1, inner.GetBySlugCalls);
    }

    [Fact]
    public async Task GetBySlug_MissingSlug_IsNegativeCached()
    {
        var (cached, inner) = CreateSut();

        var first = await cached.GetBySlugAsync("does-not-exist");
        var second = await cached.GetBySlugAsync("does-not-exist");

        Assert.Null(first);
        Assert.Null(second);
        Assert.Equal(1, inner.GetBySlugCalls);
    }

    [Fact]
    public async Task ListPublished_SecondCall_DoesNotHitInnerRepository()
    {
        var (cached, inner) = CreateSut();
        await inner.AddAsync(CreatePost());

        var first = await cached.ListPublishedAsync();
        var second = await cached.ListPublishedAsync();

        Assert.Single(first);
        Assert.Single(second);
        Assert.Equal(1, inner.ListPublishedCalls);
    }

    [Fact]
    public async Task Add_InvalidatesSlugAndListCaches()
    {
        var (cached, inner) = CreateSut();

        // Prime both caches, including the negative entry for the future slug.
        Assert.Null(await cached.GetBySlugAsync("new-post"));
        Assert.Empty(await cached.ListPublishedAsync());

        await cached.AddAsync(CreatePost("new-post"));

        // Both reads must refetch from the inner repository and observe the new post.
        var post = await cached.GetBySlugAsync("new-post");
        var list = await cached.ListPublishedAsync();

        Assert.NotNull(post);
        Assert.Single(list);
        Assert.Equal(2, inner.GetBySlugCalls);
        Assert.Equal(2, inner.ListPublishedCalls);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetBySlug_NullOrWhitespaceSlug_ThrowsWithoutHittingInnerRepository(string? slug)
    {
        var (cached, inner) = CreateSut();

        await Assert.ThrowsAnyAsync<ArgumentException>(() => cached.GetBySlugAsync(slug!));
        Assert.Equal(0, inner.GetBySlugCalls);
    }
}
