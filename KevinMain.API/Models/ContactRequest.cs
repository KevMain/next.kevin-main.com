using System.ComponentModel.DataAnnotations;

namespace KevinMain.API.Models;

/// <summary>
/// Represents a contact form submission from the website
/// </summary>
public class ContactRequest
{
    /// <summary>
    /// The sender's name
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The sender's email address
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The subject of the message
    /// </summary>
    [Required(ErrorMessage = "Subject is required")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Subject must be between 3 and 200 characters")]
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// The message body
    /// </summary>
    [Required(ErrorMessage = "Message is required")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Message must be between 10 and 2000 characters")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp when the request was received (set by server)
    /// </summary>
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}
