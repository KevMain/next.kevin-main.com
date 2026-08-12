using KevinMain.API.Models;
using KevinMain.API.Services;

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

// Register blog post repository (in-memory, singleton so posts persist for app lifetime)
builder.Services.AddSingleton<IBlogPostRepository, InMemoryBlogPostRepository>();

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

    // Seed initial blog posts so published content is available in all environments.
    // Each post is seeded independently so one bad post cannot prevent the rest from loading,
    // and failures are logged as errors because there is no lazy fallback for blog content.
    var blogRepository = scope.ServiceProvider.GetRequiredService<IBlogPostRepository>();
    var seedPosts = BlogPostSeedData.GetPosts();
    var seededCount = 0;
    foreach (var post in seedPosts)
    {
        try
        {
            await blogRepository.AddAsync(post);
            seededCount++;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to seed blog post {Slug} on startup", post.Slug);
        }
    }

    if (seededCount == 0 && seedPosts.Count > 0)
    {
        logger.LogError("No blog posts were seeded on startup - the blog will appear empty");
    }
    else
    {
        logger.LogInformation("Seeded {SeededCount} of {TotalCount} blog posts on startup", seededCount, seedPosts.Count);
    }
}

app.Run();
