using Microsoft.AspNetCore.Mvc;
using KevinMain.API.Services;

namespace KevinMain.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CVController : ControllerBase
{
    private readonly ICVDataService _cvDataService;

    public CVController(ICVDataService cvDataService)
    {
        _cvDataService = cvDataService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var cvData = await _cvDataService.GetCVDataAsync();
        return Ok(cvData);
    }

    [HttpGet("personal")]
    public async Task<IActionResult> GetPersonalInfo()
    {
        var personalInfo = await _cvDataService.GetPersonalInfoAsync();
        return Ok(personalInfo);
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var profile = await _cvDataService.GetProfileAsync();
        return Ok(profile);
    }

    [HttpGet("experience")]
    public async Task<IActionResult> GetWorkExperience()
    {
        var experience = await _cvDataService.GetWorkExperienceAsync();
        return Ok(experience);
    }

    [HttpGet("education")]
    public async Task<IActionResult> GetEducation()
    {
        var education = await _cvDataService.GetEducationAsync();
        return Ok(education);
    }

    [HttpGet("leisure")]
    public async Task<IActionResult> GetLeisureActivities()
    {
        var leisureActivities = await _cvDataService.GetLeisureActivitiesAsync();
        return Ok(leisureActivities);
    }
}
