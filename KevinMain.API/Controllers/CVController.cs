using Microsoft.AspNetCore.Mvc;
using KevinMain.API.Models;
using KevinMain.API.Services;

namespace KevinMain.API.Controllers;

/// <summary>
/// Controller for managing CV (Curriculum Vitae) data endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CVController : ControllerBase
{
    private readonly ICVDataService _cvDataService;
    private readonly ILogger<CVController> _logger;

    public CVController(ICVDataService cvDataService, ILogger<CVController> logger)
    {
        _cvDataService = cvDataService;
        _logger = logger;
    }

    /// <summary>
    /// Gets the complete CV data including all sections
    /// </summary>
    /// <returns>Complete CV data</returns>
    /// <response code="200">Returns the complete CV data</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet]
    [ProducesResponseType(typeof(CVData), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCompleteCV()
    {
        var startTime = DateTime.UtcNow;
        try
        {
            _logger.LogInformation("CV API request received");

            var serviceStartTime = DateTime.UtcNow;
            var cvData = await _cvDataService.GetCVDataAsync();
            var serviceElapsed = (DateTime.UtcNow - serviceStartTime).TotalMilliseconds;

            _logger.LogInformation("CV data service call completed in {ElapsedMs}ms", serviceElapsed);

            var totalElapsed = (DateTime.UtcNow - startTime).TotalMilliseconds;
            _logger.LogInformation("CV API request completed in {TotalMs}ms", totalElapsed);

            return Ok(cvData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving complete CV data");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { error = "An error occurred while retrieving CV data" });
        }
    }

    /// <summary>
    /// Gets personal information section (name, contact details)
    /// </summary>
    /// <returns>Personal information</returns>
    /// <response code="200">Returns personal information</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet("personal")]
    [ProducesResponseType(typeof(PersonalInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPersonalInfo()
    {
        try
        {
            var personalInfo = await _cvDataService.GetPersonalInfoAsync();
            return Ok(personalInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving personal information");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { error = "An error occurred while retrieving personal information" });
        }
    }

    /// <summary>
    /// Gets profile section including summary, key skills, and tools
    /// </summary>
    /// <returns>Profile data with skills and tools</returns>
    /// <response code="200">Returns profile data</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet("profile")]
    [ProducesResponseType(typeof(ProfileData), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var profile = await _cvDataService.GetProfileAsync();
            return Ok(profile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving profile data");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { error = "An error occurred while retrieving profile data" });
        }
    }

    /// <summary>
    /// Gets all work experience entries
    /// </summary>
    /// <returns>List of work experience entries</returns>
    /// <response code="200">Returns work experience list</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet("experience")]
    [ProducesResponseType(typeof(List<WorkExperience>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWorkExperience()
    {
        try
        {
            var experience = await _cvDataService.GetWorkExperienceAsync();
            return Ok(experience);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving work experience");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { error = "An error occurred while retrieving work experience" });
        }
    }

    /// <summary>
    /// Gets education information
    /// </summary>
    /// <returns>Education details</returns>
    /// <response code="200">Returns education information</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet("education")]
    [ProducesResponseType(typeof(Education), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEducation()
    {
        try
        {
            var education = await _cvDataService.GetEducationAsync();
            return Ok(education);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving education information");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { error = "An error occurred while retrieving education information" });
        }
    }

    /// <summary>
    /// Gets leisure activities description
    /// </summary>
    /// <returns>Leisure activities as string</returns>
    /// <response code="200">Returns leisure activities</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet("leisure")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLeisureActivities()
    {
        try
        {
            var leisureActivities = await _cvDataService.GetLeisureActivitiesAsync();
            return Ok(leisureActivities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving leisure activities");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { error = "An error occurred while retrieving leisure activities" });
        }
    }
}
