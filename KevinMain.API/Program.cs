using Azure.Data.Tables;
using Azure.Identity;
using KevinMain.API.Models;
using KevinMain.API.Services;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Get CORS origins from configuration
var corsOrigins = builder.Configuration.GetSection("CorsOrigins").Get<string[]>() 
    ?? new[] { "https://localhost:5173", "http://localhost:5173" };

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        policy => policy.WithOrigins(corsOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Configure caching settings from appsettings.json
var cachingSettings = builder.Configuration.GetSection("CachingSettings").Get<CachingSettings>() ?? new CachingSettings();
builder.Services.AddSingleton(cachingSettings);

// Register CV data service with caching
// The base service (InMemoryCVDataService) generates the data
// The CachedCVDataService wraps it with configurable in-memory caching for fast performance
// 
// To switch to database in future:
// 1. Create DatabaseCVDataService implementing ICVDataService
// 2. Replace InMemoryCVDataService with DatabaseCVDataService below
// 3. Caching will automatically work with the database source!
builder.Services.AddSingleton<InMemoryCVDataService>();
builder.Services.AddSingleton<ICVDataService>(sp =>
{
    var innerService = sp.GetRequiredService<InMemoryCVDataService>();
    var logger = sp.GetRequiredService<ILogger<CachedCVDataService>>();
    var settings = sp.GetRequiredService<CachingSettings>();
    return new CachedCVDataService(innerService, logger, settings);
});

// Register Services data service
builder.Services.AddSingleton<IServiceDataService, InMemoryServiceDataService>();

// Blog storage configuration - validated at startup so a misconfigured deployment
// fails fast with a clear message instead of at first request.
builder.Services
    .AddOptions<BlogStorageSettings>()
    .BindConfiguration("BlogStorage")
    .ValidateDataAnnotations()
    .Validate(s => builder.Environment.IsDevelopment()
            ? !string.IsNullOrWhiteSpace(s.ConnectionString)
            : Uri.TryCreate(s.ServiceUri, UriKind.Absolute, out var serviceUri)
                && serviceUri.Scheme == Uri.UriSchemeHttps,
        "BlogStorage requires ConnectionString in Development or an absolute https ServiceUri otherwise.")
    .ValidateOnStart();

builder.Services
    .AddOptions<BlogCacheSettings>()
    .BindConfiguration("BlogCache")
    .Validate(s => s.PostDuration > TimeSpan.Zero
            && s.PublishedListDuration > TimeSpan.Zero
            && s.NotFoundDuration > TimeSpan.Zero,
        "BlogCache durations must all be positive.")
    .ValidateOnStart();
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<BlogCacheSettings>>().Value);

// TableClient construction is owned here so repositories stay free of Azure/config concerns.
// Development uses the Azurite connection string; other environments use the table service
// URI with a managed identity (ManagedIdentityCredential rather than DefaultAzureCredential:
// deterministic, no silent credential-chain fall-through).
// NOTE: assumes a system-assigned identity. If switching to a user-assigned identity,
// add its client ID to configuration and pass it to ManagedIdentityCredential.
builder.Services.AddSingleton(sp =>
{
    var storage = sp.GetRequiredService<IOptions<BlogStorageSettings>>().Value;

    return builder.Environment.IsDevelopment()
        ? new TableClient(storage.ConnectionString, storage.TableName)
        : new TableClient(new Uri(storage.ServiceUri), storage.TableName, new ManagedIdentityCredential());
});

builder.Services.AddHybridCache();

// Register blog post repository: Table Storage inner implementation wrapped in a
// HybridCache decorator (posts change infrequently, so reads are served from cache).
builder.Services.AddSingleton<TableStorageBlogPostRepository>();
builder.Services.AddSingleton<IBlogPostRepository>(sp => new CachedBlogPostRepository(
    sp.GetRequiredService<TableStorageBlogPostRepository>(),
    sp.GetRequiredService<HybridCache>(),
    sp.GetRequiredService<BlogCacheSettings>()));

// Configure Strava settings from appsettings.json
var stravaSettings = builder.Configuration.GetSection("StravaSettings").Get<StravaSettings>() ?? new StravaSettings();
builder.Services.AddSingleton(stravaSettings);

// Register Strava service with HttpClient
builder.Services.AddHttpClient<IStravaService, StravaService>();

// Register running service - uses Strava if enabled, otherwise in-memory
if (stravaSettings.Enabled)
{
    builder.Services.AddScoped<IRunningService, StravaRunningService>();
}
else
{
    builder.Services.AddSingleton<IRunningService, InMemoryRunningService>();
}

// Configure SMTP settings from appsettings.json
var smtpSettings = builder.Configuration.GetSection("SmtpSettings").Get<SmtpSettings>() ?? new SmtpSettings();
builder.Services.AddSingleton(smtpSettings);

// Register contact form service
// Automatically uses SMTP if Enabled=true in appsettings.json, otherwise falls back to logging
if (smtpSettings.Enabled)
{
    builder.Services.AddScoped<IContactService, SmtpContactService>();
}
else
{
    builder.Services.AddScoped<IContactService, LoggingContactService>();
}

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Optimize JSON serialization for performance
        options.JsonSerializerOptions.DefaultBufferSize = 16384; // 16KB buffer
    });

// Add response compression for better API performance
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "KevinMain API v1");
    });
}

app.UseCors("AllowVueApp");

app.UseResponseCompression();

app.UseHttpsRedirection();

app.UseStaticFiles(); // Enable serving static files from wwwroot

app.UseAuthorization();

app.MapControllers();

// Eagerly initialize CV cache on startup to avoid cold start delays
// This pre-populates the in-memory cache before the first request
using (var scope = app.Services.CreateScope())
{
    var cvService = scope.ServiceProvider.GetRequiredService<ICVDataService>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Pre-loading CV data cache on startup...");
        var startTime = DateTime.UtcNow;
        _ = await cvService.GetCVDataAsync();
        var elapsed = (DateTime.UtcNow - startTime).TotalMilliseconds;
        logger.LogInformation("CV data cache pre-loaded successfully in {ElapsedMs}ms", elapsed);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Failed to pre-load CV cache on startup - will load on first request");
    }

    // In Development the Azurite table is created at startup; in other environments
    // the table is provisioned by deployment/IaC (see README) so no setup happens here.
    if (app.Environment.IsDevelopment())
    {
        var tableClient = scope.ServiceProvider.GetRequiredService<TableClient>();
        await tableClient.CreateIfNotExistsAsync();
    }

    // Seed initial blog posts so published content is available in all environments.
    // Storage is persistent, so posts that already exist are an expected, non-error outcome.
    var blogRepository = scope.ServiceProvider.GetRequiredService<IBlogPostRepository>();
    var seedPosts = BlogPostSeedData.GetPosts();
    var seededCount = 0;
    var existingCount = 0;
    foreach (var post in seedPosts)
    {
        try
        {
            await blogRepository.AddAsync(post);
            seededCount++;
        }
        catch (InvalidOperationException)
        {
            // Post already exists in storage from a previous run.
            existingCount++;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to seed blog post {Slug} on startup", post.Slug);
        }
    }

    if (seededCount + existingCount == 0 && seedPosts.Count > 0)
    {
        logger.LogError("No blog posts were seeded on startup - the blog will appear empty");
    }
    else
    {
        logger.LogInformation("Blog posts on startup: {SeededCount} seeded, {ExistingCount} already present, of {TotalCount} total",
            seededCount, existingCount, seedPosts.Count);
    }
}

app.Run();
