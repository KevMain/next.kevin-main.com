using KevinMain.API.Controllers;
using KevinMain.API.Models;
using KevinMain.API.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;

namespace KevinMain.API.Tests;

public class BlogControllerTests
{
    private sealed class FakeWebHostEnvironment : IWebHostEnvironment
    {
        public string EnvironmentName { get; set; } = "Production";
        public string ApplicationName { get; set; } = "KevinMain.API.Tests";
        public string WebRootPath { get; set; } = string.Empty;
        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
        public string ContentRootPath { get; set; } = string.Empty;
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }

    private static BlogController CreateController(string environmentName, IBlogPostRepository? repository = null) =>
        new(repository ?? new InMemoryBlogPostRepository(),
            new FakeWebHostEnvironment { EnvironmentName = environmentName },
            NullLogger<BlogController>.Instance);

    private static CreateBlogPostRequest CreateRequest() => new()
    {
        Title = "Test Post",
        Slug = "test-post",
        Content = "Some content",
        IsPublished = true
    };

    [Fact]
    public async Task CreatePost_InDevelopment_Returns201()
    {
        var controller = CreateController(Environments.Development);

        var result = await controller.CreatePost(CreateRequest());

        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, created.StatusCode);
    }

    [Theory]
    [InlineData("Production")]
    [InlineData("Staging")]
    public async Task CreatePost_OutsideDevelopment_Returns404_AndDoesNotAddPost(string environmentName)
    {
        var repository = new InMemoryBlogPostRepository();
        var controller = CreateController(environmentName, repository);

        var result = await controller.CreatePost(CreateRequest());

        Assert.IsType<NotFoundResult>(result);
        Assert.Null(await repository.GetBySlugAsync("test-post"));
    }

    [Fact]
    public async Task GetBySlug_PublishedPost_ReturnsOk()
    {
        var repository = new InMemoryBlogPostRepository();
        await repository.AddAsync(new BlogPost
        {
            Id = Guid.NewGuid(),
            Title = "Published",
            Slug = "published-post",
            Content = "Content",
            PublishedAt = DateTime.UtcNow,
            IsPublished = true
        });
        var controller = CreateController("Production", repository);

        var result = await controller.GetBySlug("published-post");

        var ok = Assert.IsType<OkObjectResult>(result);
        var post = Assert.IsType<BlogPost>(ok.Value);
        Assert.Equal("published-post", post.Slug);
    }

    [Fact]
    public async Task GetBySlug_UnpublishedPost_Returns404()
    {
        var repository = new InMemoryBlogPostRepository();
        await repository.AddAsync(new BlogPost
        {
            Id = Guid.NewGuid(),
            Title = "Draft",
            Slug = "draft-post",
            Content = "Content",
            PublishedAt = null,
            IsPublished = false
        });
        var controller = CreateController("Production", repository);

        var result = await controller.GetBySlug("draft-post");

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetBySlug_UnknownSlug_Returns404()
    {
        var controller = CreateController("Production");

        var result = await controller.GetBySlug("does-not-exist");

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task ListPublished_ReturnsPublishedPosts()
    {
        var repository = new InMemoryBlogPostRepository();
        foreach (var post in BlogPostSeedData.GetPosts())
        {
            await repository.AddAsync(post);
        }
        var controller = CreateController("Production", repository);

        var result = await controller.ListPublished();

        var ok = Assert.IsType<OkObjectResult>(result);
        var posts = Assert.IsAssignableFrom<IReadOnlyList<BlogPost>>(ok.Value);
        Assert.NotEmpty(posts);
        Assert.All(posts, p => Assert.True(p.IsPublished));
    }

    [Fact]
    public void SeedData_PostsAreValidAndPublished()
    {
        var posts = BlogPostSeedData.GetPosts();

        Assert.NotEmpty(posts);
        Assert.All(posts, p =>
        {
            Assert.False(string.IsNullOrWhiteSpace(p.Title));
            Assert.False(string.IsNullOrWhiteSpace(p.Slug));
            Assert.False(string.IsNullOrWhiteSpace(p.Content));
            Assert.True(p.IsPublished);
            Assert.NotNull(p.PublishedAt);
        });
        Assert.Equal(posts.Count, posts.Select(p => p.Slug).Distinct(StringComparer.OrdinalIgnoreCase).Count());
    }
}
