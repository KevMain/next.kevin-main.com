namespace KevinMain.API.Middleware;

/// <summary>
/// Middleware to add security headers to HTTP responses
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Register callback to run right before response headers are sent.
        // This guarantees security headers are applied last and identification
        // headers stay removed, even if later middleware or the server adds them.
        context.Response.OnStarting(static state =>
        {
            var headers = ((HttpContext)state).Response.Headers;

            // X-Content-Type-Options: Prevent MIME-sniffing
            headers["X-Content-Type-Options"] = "nosniff";

            // X-Frame-Options: Prevent clickjacking
            headers["X-Frame-Options"] = "DENY";

            // X-XSS-Protection: Enable XSS filter (legacy browsers)
            headers["X-XSS-Protection"] = "1; mode=block";

            // Referrer-Policy: Control referrer information
            headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

            // Content-Security-Policy: Restrict resource loading
            headers["Content-Security-Policy"] =
                "default-src 'self'; " +
                "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
                "style-src 'self' 'unsafe-inline'; " +
                "img-src 'self' data: https:; " +
                "font-src 'self' data:; " +
                "connect-src 'self'; " +
                "frame-ancestors 'none'";

            // Permissions-Policy: Control browser features
            headers["Permissions-Policy"] =
                "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()";

            // Remove server identification headers
            headers.Remove("Server");
            headers.Remove("X-Powered-By");

            return Task.CompletedTask;
        }, context);

        await _next(context);
    }
}

/// <summary>
/// Extension method to easily add security headers middleware
/// </summary>
public static class SecurityHeadersMiddlewareExtensions
{
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SecurityHeadersMiddleware>();
    }
}
