namespace SitemateIssueTrackerApp.Models;

public class IssueModel
{
    public Guid? Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}