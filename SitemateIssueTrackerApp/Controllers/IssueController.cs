using Microsoft.AspNetCore.Mvc;
using SitemateIssueTrackerApp.Models;

namespace SitemateIssueTrackerApp.Controllers;

[ApiController]
[Route("[controller]")]
public class IssueController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<IssueController> _logger;

    public IssueController(ILogger<IssueController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<IssueModel> Get()
    {
        return Enumerable.Empty<IssueModel>();
    }
}