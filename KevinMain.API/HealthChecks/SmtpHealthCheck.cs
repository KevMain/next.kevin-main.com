using Microsoft.Extensions.Diagnostics.HealthChecks;
using KevinMain.API.Models;
using MailKit.Net.Smtp;

namespace KevinMain.API.HealthChecks;

/// <summary>
/// Health check for SMTP server connectivity
/// </summary>
public class SmtpHealthCheck : IHealthCheck
{
    private readonly SmtpSettings _settings;
    private readonly ILogger<SmtpHealthCheck> _logger;

    public SmtpHealthCheck(
        SmtpSettings settings,
        ILogger<SmtpHealthCheck> logger)
    {
        _settings = settings;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        // If SMTP is disabled, it's healthy (not required)
        if (!_settings.Enabled)
        {
            return HealthCheckResult.Healthy("SMTP is disabled (using logging fallback)");
        }

        try
        {
            using var client = new SmtpClient();
            client.Timeout = 5000; // 5 second timeout

            // Connect to SMTP server
            await client.ConnectAsync(
                _settings.Host,
                _settings.Port,
                _settings.UseSsl,
                cancellationToken);

            // If we got here, connection succeeded
            await client.DisconnectAsync(true, cancellationToken);

            return HealthCheckResult.Healthy($"SMTP server {_settings.Host}:{_settings.Port} is reachable");
        }
        catch (MailKit.Security.AuthenticationException ex)
        {
            _logger.LogWarning(ex, "SMTP health check: Authentication failed (but server is reachable)");
            // Authentication failure means server is reachable, just credentials might be wrong
            return HealthCheckResult.Degraded(
                "SMTP server is reachable but authentication may fail",
                ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SMTP health check failed");
            return HealthCheckResult.Unhealthy(
                $"SMTP server {_settings.Host}:{_settings.Port} is unreachable",
                ex);
        }
    }
}
