using System.ComponentModel.DataAnnotations;

namespace KevinMain.API.Models;

/// <summary>
/// Public site settings used when server-rendering pages (canonical URLs, structured data).
/// </summary>
public class SiteSettings
{
    /// <summary>
    /// Absolute base URL of the public site, without trailing slash (e.g. https://www.kevin-main.com).
    /// Used to build canonical URLs and JSON-LD identifiers.
    /// </summary>
    [Required]
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Site name used in page titles and JSON-LD publisher.
    /// </summary>
    public string SiteName { get; set; } = "Kevin Main";

    /// <summary>
    /// Author used when a blog post has no explicit author.
    /// </summary>
    public string DefaultAuthor { get; set; } = "Kevin Main";
}
