using Microsoft.AspNetCore.Mvc;
using KevinMain.API.Models;
using KevinMain.API.Services;

namespace KevinMain.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RunningController : ControllerBase
{
    private readonly IRunningService _runningService;
    private readonly ILogger<RunningController> _logger;
    private readonly StravaSettings _stravaSettings;

    public RunningController(
        IRunningService runningService, 
        ILogger<RunningController> logger,
        StravaSettings stravaSettings)
    {
        _runningService = runningService;
        _logger = logger;
        _stravaSettings = stravaSettings;
    }

    [HttpGet("status")]
    public ActionResult<object> GetStatus()
    {
        return Ok(new
        {
            StravaEnabled = _stravaSettings.Enabled,
            HasClientId = !string.IsNullOrEmpty(_stravaSettings.ClientId),
            HasClientSecret = !string.IsNullOrEmpty(_stravaSettings.ClientSecret),
            HasRefreshToken = !string.IsNullOrEmpty(_stravaSettings.RefreshToken),
            ClientIdLength = _stravaSettings.ClientId?.Length ?? 0,
            Message = _stravaSettings.Enabled 
                ? "Strava integration is enabled" 
                : "Using in-memory sample data"
        });
    }

    [HttpGet("activities")]
    public async Task<ActionResult<IEnumerable<RunningActivity>>> GetAllActivities()
    {
        try
        {
            var activities = await _runningService.GetAllActivitiesAsync();
            return Ok(activities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching running activities");
            return StatusCode(500, "Error fetching running activities");
        }
    }

    [HttpGet("activities/{id}")]
    public async Task<ActionResult<RunningActivity>> GetActivity(int id)
    {
        try
        {
            var activity = await _runningService.GetActivityByIdAsync(id);
            if (activity == null)
            {
                return NotFound($"Activity with ID {id} not found");
            }
            return Ok(activity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching running activity {Id}", id);
            return StatusCode(500, "Error fetching running activity");
        }
    }

    [HttpGet("pbs")]
    public async Task<ActionResult<PersonalBests>> GetPersonalBests()
    {
        try
        {
            var pbs = await _runningService.GetPersonalBestsAsync();
            return Ok(pbs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching personal bests");
            return StatusCode(500, "Error fetching personal bests");
        }
    }

    [HttpGet("recent")]
    public async Task<ActionResult<IEnumerable<RunningActivity>>> GetRecentActivities([FromQuery] int count = 5)
    {
        try
        {
            var activities = await _runningService.GetRecentActivitiesAsync(count);
            return Ok(activities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching recent running activities");
            return StatusCode(500, "Error fetching recent running activities");
        }
    }

    [HttpGet("gallery")]
    public async Task<ActionResult<IEnumerable<string>>> GetGalleryImages([FromQuery] int count = 6)
    {
        try
        {
            var images = await _runningService.GetRecentImagesAsync(count);
            return Ok(images);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching gallery images");
            return StatusCode(500, "Error fetching gallery images");
        }
    }
}
