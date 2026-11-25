using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[Route("api/v1/[controller]/[action]")]
public class FotbollController : ControllerBase
{
    private readonly ILogger<FotbollController> _logger;

    public FotbollController(ILogger<FotbollController> logger)
    {
        _logger = logger;
    }

    [HttpGet("/{team}/fixtures")]
    public IActionResult GetFixtures(string team)
    {
        return Ok("Fotboll API is running.");
    }

    [HttpGet("/{team}/results")]
    public IActionResult GetResults(string team)
    {
        return Ok("Fotboll API is running.");
    }
    [HttpGet("/{team}/summary")]
    public IActionResult GetSummary(string team)
    {
        return Ok("Fotboll API is running.");
    }
    [HttpGet("/premier-league/table")]
    public IActionResult GetStandings()
    {
        return Ok("Fotboll API is running.");
    }

}