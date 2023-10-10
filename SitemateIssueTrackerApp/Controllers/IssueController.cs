using IssueTrackerModels.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SitemateIssueTrackerApp.Issue;

namespace SitemateIssueTrackerApp.Controllers;

[ApiController]
[Route("[controller]")]
public class IssueController : ControllerBase
{

    private readonly ISender _sender;
    
    public IssueController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet("{issueId}")]
    public async Task<ActionResult<IssueModel?>> Get(Guid issueId)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var issue = await _sender.Send(new IssueMediator.GetIssueCommand(issueId));

        if (issue is null)
            return NotFound();

        return Ok(issue);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> NewIssue(IssueModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var issueId = await _sender.Send(new IssueMediator.CreateIssueCommand(model));
        
        return Ok(issueId);
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateIssue(IssueModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        await _sender.Send(new IssueMediator.UpdateIssueCommand(model));

        return Ok();
    }
    
    [HttpDelete("{issueId}")]
    public async Task<ActionResult> DeleteIssue(Guid issueId)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        await _sender.Send(new IssueMediator.DeleteIssueCommand(issueId));

        return Ok();
    }
}