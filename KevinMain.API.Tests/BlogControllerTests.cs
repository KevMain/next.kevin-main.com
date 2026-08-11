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
}
