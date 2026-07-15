using KevinMain.API.Models;

namespace KevinMain.API.Services;

/// <summary>
/// Initial implementation of IContactService that logs contact requests to the console.
/// This can be swapped for a real email service implementation without changing the controller.
/// </summary>
public class LoggingContactService : IContactService
{
    private readonly ILogger<LoggingContactService> _logger;

    public LoggingContactService(ILogger<LoggingContactService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Logs the contact request details to console.
    /// In a real implementation, this is where you would:
    /// - Send email via SMTP, SendGrid, Mailgun, etc.
    /// - Store the message in a database
    /// - Trigger notifications
    /// - Apply rate limiting/spam detection
    /// </summary>
    public async Task<ContactResult> ProcessContactRequestAsync(ContactRequest request)
    {
        try
        {
            // Log the contact request
            _logger.LogInformation(
                "Contact form submission received:\n" +
                "  From: {Name} <{Email}>\n" +
                "  Subject: {Subject}\n" +
                "  Message: {Message}\n" +
                "  Submitted: {SubmittedAt}",
                request.Name,
                request.Email,
                request.Subject,
                request.Message,
                request.SubmittedAt
            );

            // Simulate async work (remove in real implementation)
            await Task.Delay(100);

            // TODO: Replace this with real email sending logic
            // Example for future SMTP implementation:
            // await _smtpClient.SendEmailAsync(
            //     to: "your-email@example.com",
            //     from: request.Email,
            //     subject: $"Contact Form: {request.Subject}",
            //     body: $"From: {request.Name} ({request.Email})\n\n{request.Message}"
            // );

            return new ContactResult
            {
                Success = true,
                Message = "Thank you for your message! I'll get back to you soon."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing contact request from {Email}", request.Email);

            return new ContactResult
            {
                Success = false,
                Message = "Sorry, there was an error sending your message. Please try again later.",
                ErrorDetails = ex.Message
            };
        }
    }
}
