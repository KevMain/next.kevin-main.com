namespace KevinMain.API.Models;

/// <summary>
/// Configuration settings for SMTP email sending
/// </summary>
public class SmtpSettings
{
    /// <summary>
    /// SMTP server host (e.g., smtp.gmail.com, smtp.office365.com)
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// SMTP server port (typically 587 for TLS, 465 for SSL, 25 for unsecured)
    /// </summary>
    public int Port { get; set; } = 587;

    /// <summary>
    /// Whether to use SSL/TLS encryption (recommended: true)
    /// </summary>
    public bool UseSsl { get; set; } = true;

    /// <summary>
    /// SMTP username (usually your email address)
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// SMTP password or app-specific password
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Email address to send FROM (usually same as Username)
    /// </summary>
    public string FromEmail { get; set; } = string.Empty;

    /// <summary>
    /// Display name for the FROM field (e.g., "Kevin Main Portfolio")
    /// </summary>
    public string FromName { get; set; } = string.Empty;

    /// <summary>
    /// Email address to send contact form submissions TO
    /// </summary>
    public string ToEmail { get; set; } = string.Empty;

    /// <summary>
    /// Whether SMTP is enabled (set to false to use LoggingContactService instead)
    /// </summary>
    public bool Enabled { get; set; } = false;
}
