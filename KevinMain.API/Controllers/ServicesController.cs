using Microsoft.AspNetCore.Mvc;
using KevinMain.API.Models;
using KevinMain.API.Services;

namespace KevinMain.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IServiceDataService _serviceDataService;

    public ServicesController(IServiceDataService serviceDataService)
    {
        _serviceDataService = serviceDataService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var serviceData = await _serviceDataService.GetServiceDataAsync();
        return Ok(serviceData);
    }

    [HttpGet("category/{categoryName}")]
    public async Task<IActionResult> GetByCategory(string categoryName)
    {
        var serviceData = await _serviceDataService.GetServiceDataAsync();
        var category = serviceData.Categories
            .FirstOrDefault(c => c.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

        if (category == null)
        {
            return NotFound(new { message = $"Category '{categoryName}' not found" });
        }

        return Ok(category);
    }
}
