using KevinMain.API.Models;
using KevinMain.API.Services;

namespace KevinMain.API.Tests;

public class InMemoryBlogPostRepositoryTests
{
    private static BlogPost CreatePost(string slug, bool isPublished = true, DateTime? publishedAt = null) => new()
    {
        Id = Guid.NewGuid(),
        Title = $"Title for {slug}",
        Slug = slug,
        Content = "Some content",
        PublishedAt = publishedAt ?? DateTime.UtcNow,
        IsPublished = isPublished
    };

    [Fact]
    public async Task AddAsync_AddsPost_AndReturnsIt()
    {
        var repository = new InMemoryBlogPostRepository();
        var post = CreatePost("my-first-post");

        var result = await repository.AddAsync(post);

        Assert.Same(post, result);
        var stored = await repository.GetBySlugAsync("my-first-post");
        Assert.NotNull(stored);
        Assert.Equal(post.Id, stored.Id);
    }

    [Fact]
    public async Task AddAsync_DuplicateSlug_Throws()
    {
        var repository = new InMemoryBlogPostRepository();
        await repository.AddAsync(CreatePost("duplicate-slug"));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => repository.AddAsync(CreatePost("duplicate-slug")));
    }

    [Fact]
    public async Task GetBySlugAsync_ReturnsPost_WhenExists()
    {
        var repository = new InMemoryBlogPostRepository();
        var post = CreatePost("find-me");
        await repository.AddAsync(post);

        var result = await repository.GetBySlugAsync("find-me");

        Assert.NotNull(result);
        Assert.Equal(post.Id, result.Id);
        Assert.Equal("find-me", result.Slug);
    }

    [Fact]
    public async Task GetBySlugAsync_ReturnsNull_WhenNotFound()
    {
        var repository = new InMemoryBlogPostRepository();

        var result = await repository.GetBySlugAsync("does-not-exist");

        Assert.Null(result);
    }

    [Fact]
    public async Task ListPublishedAsync_ReturnsOnlyPublishedPosts()
    {
        var repository = new InMemoryBlogPostRepository();
        await repository.AddAsync(CreatePost("published-one", isPublished: true));
        await repository.AddAsync(CreatePost("draft-one", isPublished: false));
        await repository.AddAsync(CreatePost("published-two", isPublished: true));

        var result = await repository.ListPublishedAsync();

        Assert.Equal(2, result.Count);
        Assert.All(result, p => Assert.True(p.IsPublished));
        Assert.DoesNotContain(result, p => p.Slug == "draft-one");
    }

    [Fact]
    public async Task ListPublishedAsync_OrdersNewestFirst()
    {
        var repository = new InMemoryBlogPostRepository();
        var older = CreatePost("older", publishedAt: DateTime.UtcNow.AddDays(-2));
        var newer = CreatePost("newer", publishedAt: DateTime.UtcNow);
        await repository.AddAsync(older);
        await repository.AddAsync(newer);

        var result = await repository.ListPublishedAsync();

        Assert.Equal("newer", result[0].Slug);
        Assert.Equal("older", result[1].Slug);
    }
}
