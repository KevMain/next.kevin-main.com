using KevinMain.API.Models;

namespace KevinMain.API.Services;

/// <summary>
/// Service interface for handling contact form submissions.
/// This abstraction allows swapping between different implementations:
/// - LoggingContactService (current - logs to console)
/// - SmtpContactService (future - sends via SMTP)
/// - SendGridContactService (future - sends via SendGrid API)
/// - DatabaseContactService (future - stores in database and sends email)
/// </summary>
public interface IContactService
{
    /// <summary>
    /// Process a contact form submission.
    /// Implementation may send email, store in database, log, or any combination.
    /// </summary>
    /// <param name="request">The contact form data</param>
    /// <returns>Task representing the async operation</returns>
    Task<ContactResult> ProcessContactRequestAsync(ContactRequest request);
}

/// <summary>
/// Result of processing a contact request
/// </summary>
public class ContactResult
{
    /// <summary>
    /// Whether the request was successfully processed
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Message to return to the user
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Optional error details (for logging/debugging)
    /// </summary>
    public string? ErrorDetails { get; set; }
}
