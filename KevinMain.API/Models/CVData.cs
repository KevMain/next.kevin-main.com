namespace KevinMain.API.Models;

public class CVData
{
    public PersonalInfo PersonalInfo { get; set; } = new();
    public string Profile { get; set; } = string.Empty;
    public List<string> KeySkills { get; set; } = new();
    public List<string> Tools { get; set; } = new();
    public List<WorkExperience> WorkExperience { get; set; } = new();
    public Education Education { get; set; } = new();
    public string LeisureActivities { get; set; } = string.Empty;
}

public class PersonalInfo
{
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}

public class WorkExperience
{
    public string Company { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Highlights { get; set; } = new();
    public string TechStack { get; set; } = string.Empty;
}

public class Education
{
    public HigherEducation Higher { get; set; } = new();
    public List<SecondaryEducation> Secondary { get; set; } = new();
}

public class HigherEducation
{
    public string University { get; set; } = string.Empty;
    public string Course { get; set; } = string.Empty;
    public string Grade { get; set; } = string.Empty;
    public string Dates { get; set; } = string.Empty;
}

public class SecondaryEducation
{
    public string Institution { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public string Grade { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
}
