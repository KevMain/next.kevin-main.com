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

    /// <summary>
    /// Gets personal information (name, contact details).
    /// </summary>
    Task<PersonalInfo> GetPersonalInfoAsync();

    /// <summary>
    /// Gets profile summary including profile text, key skills, and tools.
    /// </summary>
    Task<ProfileData> GetProfileAsync();

    /// <summary>
    /// Gets all work experience entries.
    /// </summary>
    Task<List<WorkExperience>> GetWorkExperienceAsync();

    /// <summary>
    /// Gets education information.
    /// </summary>
    Task<Education> GetEducationAsync();

    /// <summary>
    /// Gets leisure activities description.
    /// </summary>
    Task<string> GetLeisureActivitiesAsync();
}
