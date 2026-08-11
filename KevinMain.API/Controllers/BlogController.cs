using Microsoft.AspNetCore.Mvc;
using KevinMain.API.Models;
using KevinMain.API.Services;

namespace KevinMain.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private readonly IBlogPostRepository _repository;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<BlogController> _logger;

    public BlogController(IBlogPostRepository repository, IWebHostEnvironment environment, ILogger<BlogController> logger)
    {
        _repository = repository;
        _environment = environment;
        _logger = logger;
    }

    /// <summary>
    /// Create a new blog post. Only available in the Development environment.
    /// </summary>
    /// <param name="request">The blog post data</param>
    /// <returns>The created blog post</returns>
    [HttpPost]
    [ProducesResponseType(typeof(BlogPost), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreatePost([FromBody] CreateBlogPostRequest request)
    {
        // Unauthenticated write endpoint - restricted to Development so it cannot be
        // used to create arbitrary posts (or grow the in-memory store) in production.
        if (!_environment.IsDevelopment())
        {
            _logger.LogWarning("Blocked blog post creation attempt outside Development environment");
            return NotFound();
        }

        var post = new BlogPost
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Slug = request.Slug,
            Content = request.Content,
            PublishedAt = request.IsPublished ? DateTime.UtcNow : null,
            IsPublished = request.IsPublished
        };

        try
        {
            var created = await _repository.AddAsync(post);
            _logger.LogInformation("Created blog post {Slug} ({Id})", created.Slug, created.Id);
            return CreatedAtAction(nameof(GetBySlug), new { slug = created.Slug }, created);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Failed to create blog post: {Message}", ex.Message);
            return Conflict(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get a blog post by slug
    /// </summary>
    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(BlogPost), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var post = await _repository.GetBySlugAsync(slug);
        return post is null ? NotFound() : Ok(post);
    }

    /// <summary>
    /// List all published blog posts
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<BlogPost>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListPublished()
    {
        var posts = await _repository.ListPublishedAsync();
        return Ok(posts);
    }
}
