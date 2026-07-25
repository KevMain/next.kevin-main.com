using System.ComponentModel.DataAnnotations;

namespace KevinMain.API.Models;

/// <summary>
/// Complete CV data structure
/// </summary>
public class CVData
{
    [Required]
    public PersonalInfo PersonalInfo { get; set; } = new();

    [Required]
    [StringLength(5000, MinimumLength = 10, ErrorMessage = "Profile must be between 10 and 5000 characters")]
    public string Profile { get; set; } = string.Empty;

    [Required]
    [MinLength(1, ErrorMessage = "At least one key skill is required")]
    public List<string> KeySkills { get; set; } = new();

    public List<string> Tools { get; set; } = new();

    [Required]
    [MinLength(1, ErrorMessage = "At least one work experience entry is required")]
    public List<WorkExperience> WorkExperience { get; set; } = new();

    [Required]
    public Education Education { get; set; } = new();

    [StringLength(2000, ErrorMessage = "Leisure activities cannot exceed 2000 characters")]
    public string LeisureActivities { get; set; } = string.Empty;
}

/// <summary>
/// Personal contact information
/// </summary>
public class PersonalInfo
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 200 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number")]
    [StringLength(50, ErrorMessage = "Phone cannot exceed 50 characters")]
    public string Phone { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
    public string Location { get; set; } = string.Empty;
}

/// <summary>
/// Work experience entry
/// </summary>
public class WorkExperience
{
    [Required(ErrorMessage = "Company name is required")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 200 characters")]
    public string Company { get; set; } = string.Empty;

    [Required(ErrorMessage = "Position is required")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Position must be between 2 and 200 characters")]
    public string Position { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start date is required")]
    [StringLength(50, ErrorMessage = "Start date cannot exceed 50 characters")]
    public string StartDate { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "End date cannot exceed 50 characters")]
    public string EndDate { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
    public string Location { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;

    public List<string> Highlights { get; set; } = new();

    [StringLength(500, ErrorMessage = "Tech stack cannot exceed 500 characters")]
    public string TechStack { get; set; } = string.Empty;
}

/// <summary>
/// Education information
/// </summary>
public class Education
{
    public HigherEducation Higher { get; set; } = new();
    public List<SecondaryEducation> Secondary { get; set; } = new();
}

/// <summary>
/// Higher education details (University/College)
/// </summary>
public class HigherEducation
{
    [StringLength(200, ErrorMessage = "University name cannot exceed 200 characters")]
    public string University { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Course name cannot exceed 200 characters")]
    public string Course { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Grade cannot exceed 100 characters")]
    public string Grade { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Dates cannot exceed 50 characters")]
    public string Dates { get; set; } = string.Empty;
}

/// <summary>
/// Secondary education details (A-Levels, BTEC, etc.)
/// </summary>
public class SecondaryEducation
{
    [StringLength(200, ErrorMessage = "Institution name cannot exceed 200 characters")]
    public string Institution { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Qualification cannot exceed 200 characters")]
    public string Qualification { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Grade cannot exceed 100 characters")]
    public string Grade { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Date cannot exceed 50 characters")]
    public string Date { get; set; } = string.Empty;
}
