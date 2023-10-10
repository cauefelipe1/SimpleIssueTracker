namespace SitemateIssueTrackerApp.Issue;


public class IssueStaticRepository : IIssueRepository
{
    private List<IssueDto> _recordedIssues = new();

    public IssueDto? GetIssue(Guid id)
    {
        var issue = _recordedIssues.FirstOrDefault(i => i.IssueId == id);

        return issue;
    }

    public Guid CreateIssue(IssueDto dto)
    {
        var newDto = new IssueDto
        {
            IssueId = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description
        };
        
        _recordedIssues.Add(newDto);

        return newDto.IssueId;
    }

    public void UpdateIssue(IssueDto dto)
    {
        var issue = GetIssue(dto.IssueId);

        if (issue is null)
            throw new Exception($"The issue {dto.IssueId} was not found.");

        issue.Title = dto.Title;
        issue.Description = dto.Description;
    }

    public void DeleteIssue(Guid id)
    {
        var issue = GetIssue(id);

        if (issue is null)
            throw new Exception($"The issue {id} was not found.");

        _recordedIssues.Remove(issue);
    }
}