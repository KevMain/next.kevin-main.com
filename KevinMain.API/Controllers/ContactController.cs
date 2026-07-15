using Microsoft.AspNetCore.Mvc;
using KevinMain.API.Models;
using KevinMain.API.Services;

namespace KevinMain.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly ILogger<ContactController> _logger;

    public ContactController(IContactService contactService, ILogger<ContactController> logger)
    {
        _contactService = contactService;
        _logger = logger;
    }

    /// <summary>
    /// Submit a contact form message
    /// </summary>
    /// <param name="request">The contact form data</param>
    /// <returns>Result indicating success or failure</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ContactResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SubmitContact([FromBody] ContactRequest request)
    {
        // Model validation is handled automatically by [ApiController] attribute
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid contact form submission: {Errors}", 
                string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            return BadRequest(ModelState);
        }

        // TODO: Add rate limiting here to prevent spam
        // Example: Check if IP has submitted more than 5 messages in last hour

        var result = await _contactService.ProcessContactRequestAsync(request);

        if (result.Success)
        {
            _logger.LogInformation("Contact form processed successfully for {Email}", request.Email);
            return Ok(result);
        }
        else
        {
            _logger.LogError("Failed to process contact form for {Email}: {Error}", 
                request.Email, result.ErrorDetails);
            return StatusCode(500, result);
        }
    }

    /// <summary>
    /// Health check endpoint for contact form functionality
    /// </summary>
    [HttpGet("health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Health()
    {
        return Ok(new { status = "healthy", service = "contact" });
    }
}
