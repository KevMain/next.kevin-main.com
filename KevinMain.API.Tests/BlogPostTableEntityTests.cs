using KevinMain.API.Models;
using KevinMain.API.Services;

namespace KevinMain.API.Tests;

public class BlogPostTableEntityTests
{
    private static BlogPost CreatePost(bool published = true) => new()
    {
        Id = Guid.NewGuid(),
        Title = "My Post",
        Slug = "my-post",
        Content = "Hello **world**",
        PublishedAt = published ? new DateTime(2026, 1, 15, 10, 30, 0, DateTimeKind.Utc) : null,
        IsPublished = published
    };

    [Fact]
    public void FromBlogPost_MapsAllProperties()
    {
        var post = CreatePost();

        var entity = BlogPostTableEntity.FromBlogPost(post);

        Assert.Equal(BlogPostTableEntity.BlogPartitionKey, entity.PartitionKey);
        Assert.Equal("my-post", entity.RowKey);
        Assert.Equal(post.Id, entity.Id);
        Assert.Equal(post.Title, entity.Title);
        Assert.Equal(post.Slug, entity.Slug);
        Assert.Equal(post.Content, entity.Content);
        Assert.Equal(post.PublishedAt, entity.PublishedAt);
        Assert.True(entity.IsPublished);
    }

    [Fact]
    public void RoundTrip_PreservesBlogPost()
    {
        var post = CreatePost();

        var roundTripped = BlogPostTableEntity.FromBlogPost(post).ToBlogPost();

        Assert.Equal(post.Id, roundTripped.Id);
        Assert.Equal(post.Title, roundTripped.Title);
        Assert.Equal(post.Slug, roundTripped.Slug);
        Assert.Equal(post.Content, roundTripped.Content);
        Assert.Equal(post.PublishedAt, roundTripped.PublishedAt);
        Assert.Equal(post.IsPublished, roundTripped.IsPublished);
    }

    [Fact]
    public void RoundTrip_Draft_PreservesNullPublishedAt()
    {
        var draft = CreatePost(published: false);

        var roundTripped = BlogPostTableEntity.FromBlogPost(draft).ToBlogPost();

        Assert.Null(roundTripped.PublishedAt);
        Assert.False(roundTripped.IsPublished);
    }

    [Fact]
    public void FromBlogPost_NormalizesRowKeyToLowercase()
    {
        var post = CreatePost();
        post.Slug = "MY-POST";

        var entity = BlogPostTableEntity.FromBlogPost(post);

        Assert.Equal("my-post", entity.RowKey);
        Assert.Equal("MY-POST", entity.Slug);
    }
}
