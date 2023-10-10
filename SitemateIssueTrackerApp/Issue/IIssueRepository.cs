namespace SitemateIssueTrackerApp.Issue;

public interface IIssueRepository
{
    IssueDto? GetIssue(Guid id);
    Guid CreateIssue(IssueDto dto);
    void UpdateIssue(IssueDto dto);
    void DeleteIssue(Guid id);
}