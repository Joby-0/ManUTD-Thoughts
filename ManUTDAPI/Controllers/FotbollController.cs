using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FotbollController : ControllerBase
{
    private readonly ILogger<FotbollController> _logger;
    private readonly IFotbollService _fotbollService;

    public FotbollController(ILogger<FotbollController> logger, IFotbollService fotbollService)
    {
        _logger = logger;
        _fotbollService = fotbollService;
    }

    [HttpGet("/{team}/fixtures")]
    public async Task<IActionResult> GetFixtures(string team)
    {
        var fixtures = await _fotbollService.GetFixturesAsync(team);
        return Ok(fixtures);
    }

    [HttpGet("/{team}/results")]
    public async Task<IActionResult> GetResults(string team)
    {
        var results = await _fotbollService.GetResultsAsync(team);
        return Ok(results);
    }
    [HttpGet("/{team}/summary")]
    public async Task<IActionResult> GetSummary(string team)
    {
        var summary = await _fotbollService.GetSummaryAsync();
        return Ok(summary);
    }
    [HttpGet("/premier-league/table")]
    public async Task<IActionResult> GetStandings()
    {
        var standings = await _fotbollService.GetPremierLeagueStandingsAsync();
        return Ok(standings);
    }
}