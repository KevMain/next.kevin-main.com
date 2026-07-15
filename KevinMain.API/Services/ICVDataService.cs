using KevinMain.API.Models;

namespace KevinMain.API.Services;

/// <summary>
/// Service interface for retrieving CV data.
/// This abstraction allows easy swapping between in-memory, database, or external API implementations.
/// </summary>
public interface ICVDataService
{
    /// <summary>
    /// Gets the complete CV data.
    /// Future implementations could make this async and retrieve from a database.
    /// </summary>
    Task<CVData> GetCVDataAsync();
}
