namespace SitemateIssueTrackerApp.Issue;

public class IssueDto
{
    public Guid IssueId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}