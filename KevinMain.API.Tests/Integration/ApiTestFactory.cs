using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace KevinMain.API.Tests.Integration;

/// <summary>
/// Custom WebApplicationFactory that disables rate limiting so tests
/// aren't throttled by the IpRateLimiting rules in appsettings.json.
/// </summary>
public class ApiTestFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureTestServices(services =>
        {
            // Clear all rate limit rules so integration tests aren't throttled
            services.PostConfigure<IpRateLimitOptions>(options =>
            {
                options.GeneralRules = new List<RateLimitRule>();
                options.EndpointWhitelist = new List<string> { "*" };
            });

            // Disable SMTP so the smtp_server health check doesn't depend on
            // an external mail server being reachable from the test environment
            services.AddSingleton(new KevinMain.API.Models.SmtpSettings { Enabled = false });

            // Use the logging contact service so tests never send real email
            services.AddScoped<KevinMain.API.Services.IContactService,
                KevinMain.API.Services.LoggingContactService>();

            // Force Strava off and pin the in-memory running service so tests
            // are deterministic and never call the real Strava API, regardless
            // of what appsettings configure at startup
            services.AddSingleton(new KevinMain.API.Models.StravaSettings { Enabled = false });
            services.AddSingleton<KevinMain.API.Services.IRunningService,
                KevinMain.API.Services.InMemoryRunningService>();
        });
    }
}
