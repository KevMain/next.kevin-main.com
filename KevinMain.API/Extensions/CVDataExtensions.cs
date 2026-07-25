using KevinMain.API.Models;

namespace KevinMain.API.Extensions;

/// <summary>
/// Extension methods for CVData models
/// </summary>
public static class CVDataExtensions
{
    /// <summary>
    /// Converts CVData to ProfileData projection containing profile summary, skills, and tools.
    /// </summary>
    /// <param name="cvData">The source CV data</param>
    /// <returns>ProfileData projection</returns>
    public static ProfileData ToProfileData(this CVData cvData)
    {
        return new ProfileData
        {
            Profile = cvData.Profile,
            KeySkills = cvData.KeySkills,
            Tools = cvData.Tools
        };
    }
}
