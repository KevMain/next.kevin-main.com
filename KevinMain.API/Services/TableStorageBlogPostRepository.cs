using Azure;
using Azure.Data.Tables;
using KevinMain.API.Models;

namespace KevinMain.API.Services;

/// <summary>
/// Blog post repository backed by Azure Table Storage.
/// Pure data access: the <see cref="TableClient"/> is constructed and configured
/// in Program.cs, and the table itself is provisioned by deployment/IaC
/// (or a Development-only startup step for Azurite) — never here.
/// </summary>
public class TableStorageBlogPostRepository : IBlogPostRepository
{
    private readonly TableClient _tableClient;

    public TableStorageBlogPostRepository(TableClient tableClient)
    {
        _tableClient = tableClient;
    }

    public async Task<BlogPost> AddAsync(BlogPost post)
    {
        ArgumentNullException.ThrowIfNull(post);
        ArgumentException.ThrowIfNullOrWhiteSpace(post.Slug);

        var entity = BlogPostTableEntity.FromBlogPost(post);

        try
        {
            await _tableClient.AddEntityAsync(entity);
        }
        catch (RequestFailedException ex) when (ex.Status == StatusCodes.Status409Conflict)
        {
            throw new InvalidOperationException($"A blog post with slug '{post.Slug}' already exists.");
        }

        return post;
    }

    public async Task<BlogPost?> GetBySlugAsync(string slug)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        // Point lookup by PartitionKey + RowKey — the access pattern Table Storage
        // is designed for. GetEntityIfExistsAsync treats "not found" as a normal
        // result rather than exception-driven control flow.
        var response = await _tableClient.GetEntityIfExistsAsync<BlogPostTableEntity>(
            BlogPostTableEntity.BlogPartitionKey,
            BlogPostTableEntity.NormalizeSlug(slug));

        return response.HasValue ? response.Value!.ToBlogPost() : null;
    }

    public async Task<IReadOnlyList<BlogPost>> ListPublishedAsync()
    {
        var published = new List<BlogPost>();

        await foreach (var entity in _tableClient.QueryAsync<BlogPostTableEntity>(
            e => e.PartitionKey == BlogPostTableEntity.BlogPartitionKey && e.IsPublished))
        {
            published.Add(entity.ToBlogPost());
        }

        // Intentional in-memory sort: Table Storage only orders results by
        // PartitionKey/RowKey and has no server-side OrderBy, so sorting by
        // PublishedAt after retrieval is the correct approach at this scale.
        return published
            .OrderByDescending(p => p.PublishedAt)
            .ToList();
    }
}
