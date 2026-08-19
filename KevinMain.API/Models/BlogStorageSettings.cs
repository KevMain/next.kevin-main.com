using System.ComponentModel.DataAnnotations;

namespace KevinMain.API.Models;

/// <summary>
/// Settings for the Azure Table Storage account backing blog posts.
/// Development uses <see cref="ConnectionString"/> (Azurite);
/// other environments use <see cref="ServiceUri"/> with a managed identity.
/// </summary>
public class BlogStorageSettings
{
    /// <summary>
    /// Storage connection string. Required in Development (Azurite).
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Table service URI (e.g. https://account.table.core.windows.net).
    /// Required outside Development; authenticated via managed identity.
    /// </summary>
    public string ServiceUri { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string TableName { get; set; } = "blogposts";
}
