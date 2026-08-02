using KevinMain.API.Models;
using KevinMain.API.Services;
using AspNetCoreRateLimit;
using KevinMain.API.Middleware;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add memory cache for rate limiting
builder.Services.AddMemoryCache();

// Configure IP rate limiting
builder.Services.AddHttpContextAccessor(); // required by AspNetCoreRateLimit
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();

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

// Add health checks
builder.Services.AddHealthChecks()
    .AddCheck<KevinMain.API.HealthChecks.StravaHealthCheck>(
        "strava_api",
        tags: new[] { "external", "api" })
    .AddCheck<KevinMain.API.HealthChecks.SmtpHealthCheck>(
        "smtp_server",
        tags: new[] { "external", "email" });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Add security headers
app.UseSecurityHeaders();

// Resolve the real client IP from X-Forwarded-For set by the Azure Container
// Apps ingress proxy. Only enabled outside Development: locally there is no
// trusted proxy, so honoring forwarded headers would let clients spoof their
// IP/scheme and undermine rate limiting. ForwardLimit=1 means only the
// nearest (ingress-appended) entry is trusted, not arbitrary client-supplied
// chains. RealIpHeader is deliberately set to an inert header name in
// appsettings.json so the spoofable X-Real-IP header is never consulted.
if (!app.Environment.IsDevelopment())
{
    var forwardedHeadersOptions = new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        ForwardLimit = 1
    };
    // All production traffic reaches this app via the ACA ingress; accept its forwarded headers
    forwardedHeadersOptions.KnownNetworks.Clear();
    forwardedHeadersOptions.KnownProxies.Clear();
    app.UseForwardedHeaders(forwardedHeadersOptions);
}

// CORS must run before rate limiting so throttled (429) responses
// still carry CORS headers and aren't blocked by browsers
app.UseCors("AllowVueApp");

// Apply IP rate limiting middleware
app.UseIpRateLimiting();

app.UseResponseCompression();

// HTTPS redirection for production
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles(); // Enable serving static files from wwwroot

app.UseAuthorization();

app.MapControllers();

// Map health check endpoints
// Basic liveness endpoint for Azure Container Apps probes.
// Excludes checks tagged "external" (Strava/SMTP) so a temporarily
// unavailable dependency can't cause unnecessary container restarts.
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = registration => !registration.Tags.Contains("external")
});

// Detailed endpoint with full health check information.
// Descriptions of "external" checks are redacted because they can contain
// infrastructure details (e.g. SMTP host/port); full messages remain in logs.
app.MapHealthChecks("/health/detailed", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Tags.Contains("external") ? null : e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds,
                tags = e.Value.Tags
            }),
            totalDuration = report.TotalDuration.TotalMilliseconds
        }, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });

        await context.Response.WriteAsync(result);
    }
});

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
}

app.Run();

// Make the implicit Program class public so integration tests can reference it
public partial class Program { }
