namespace KevinMain.API.Models;

/// <summary>
/// Profile data including summary, skills, and tools
/// </summary>
public class ProfileData
{
    public string Profile { get; set; } = string.Empty;
    public List<string> KeySkills { get; set; } = new();
    public List<string> Tools { get; set; } = new();
}
