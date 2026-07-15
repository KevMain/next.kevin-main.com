using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using KevinMain.API.Models;

namespace KevinMain.API.Services;

/// <summary>
/// Implementation of IContactService that sends emails via SMTP using MailKit.
/// Supports Gmail, Outlook, and other standard SMTP providers.
/// </summary>
public class SmtpContactService : IContactService
{
    private readonly SmtpSettings _settings;
    private readonly ILogger<SmtpContactService> _logger;

    public SmtpContactService(SmtpSettings settings, ILogger<SmtpContactService> logger)
    {
        _settings = settings;
        _logger = logger;
    }

    /// <summary>
    /// Sends a contact form submission via SMTP email
    /// </summary>
    public async Task<ContactResult> ProcessContactRequestAsync(ContactRequest request)
    {
        try
        {
            _logger.LogInformation(
                "Processing contact form via SMTP from {Email} with subject: {Subject}",
                request.Email,
                request.Subject
            );

            // Create email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            message.To.Add(new MailboxAddress("", _settings.ToEmail));
            message.Subject = $"Contact Form: {request.Subject}";

            // Build email body with contact form details
            var bodyBuilder = new BodyBuilder
            {
                TextBody = $@"
New contact form submission received:

From: {request.Name}
Email: {request.Email}
Subject: {request.Subject}
Submitted: {request.SubmittedAt:yyyy-MM-dd HH:mm:ss} UTC

Message:
{request.Message}

---
This message was sent from the contact form at kevin-main.com
Reply to: {request.Email}
",
                HtmlBody = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #0ea5e9 0%, #a855f7 100%); color: white; padding: 20px; border-radius: 8px 8px 0 0; }}
        .content {{ background: #f8f9fa; padding: 20px; border: 1px solid #e0e0e0; }}
        .field {{ margin-bottom: 15px; }}
        .field-label {{ font-weight: bold; color: #555; }}
        .field-value {{ margin-top: 5px; padding: 10px; background: white; border-radius: 4px; border-left: 3px solid #0ea5e9; }}
        .message-box {{ margin-top: 10px; padding: 15px; background: white; border-radius: 4px; border: 1px solid #e0e0e0; white-space: pre-wrap; }}
        .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #888; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>📬 New Contact Form Submission</h2>
        </div>
        <div class='content'>
            <div class='field'>
                <div class='field-label'>From:</div>
                <div class='field-value'>{request.Name}</div>
            </div>
            <div class='field'>
                <div class='field-label'>Email:</div>
                <div class='field-value'><a href='mailto:{request.Email}'>{request.Email}</a></div>
            </div>
            <div class='field'>
                <div class='field-label'>Subject:</div>
                <div class='field-value'>{request.Subject}</div>
            </div>
            <div class='field'>
                <div class='field-label'>Submitted:</div>
                <div class='field-value'>{request.SubmittedAt:yyyy-MM-dd HH:mm:ss} UTC</div>
            </div>
            <div class='field'>
                <div class='field-label'>Message:</div>
                <div class='message-box'>{System.Net.WebUtility.HtmlEncode(request.Message)}</div>
            </div>
        </div>
        <div class='footer'>
            <p>This message was sent from the contact form at kevin-main.com</p>
            <p><a href='mailto:{request.Email}'>Reply to {request.Email}</a></p>
        </div>
    </div>
</body>
</html>
"
            };

            message.Body = bodyBuilder.ToMessageBody();

            // Send email via SMTP
            using (var client = new SmtpClient())
            {
                // Connect to SMTP server
                await client.ConnectAsync(_settings.Host, _settings.Port, 
                    _settings.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);

                // Authenticate
                await client.AuthenticateAsync(_settings.Username, _settings.Password);

                // Send message
                await client.SendAsync(message);

                // Disconnect
                await client.DisconnectAsync(true);
            }

            _logger.LogInformation(
                "Contact form email sent successfully to {ToEmail} from {From}",
                _settings.ToEmail,
                request.Email
            );

            return new ContactResult
            {
                Success = true,
                Message = "Thank you for your message! I'll get back to you soon."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Failed to send contact form email from {Email}. SMTP: {Host}:{Port}",
                request.Email,
                _settings.Host,
                _settings.Port
            );

            return new ContactResult
            {
                Success = false,
                Message = "Sorry, there was an error sending your message. Please try again later.",
                ErrorDetails = ex.Message
            };
        }
    }
}
