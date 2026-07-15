using Microsoft.AspNetCore.Mvc;
using KevinMain.API.Models;
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
        // Data is now retrieved from the service layer
        // To switch to database: Just change the service registration in Program.cs
        // No changes needed to this controller!
        var cvData = await _cvDataService.GetCVDataAsync();
        return Ok(cvData);
    }
}
