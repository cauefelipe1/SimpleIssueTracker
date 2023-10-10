using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using IssueTrackerModels.Models;

namespace IssueTrackerApiClient;

public class IssueTrackerClient
{
    private readonly string _baseUrl = "http://localhost:5274/Issue";
    private Guid? _issueId = null;

    private HttpClient _httpClient;

    public IssueTrackerClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_baseUrl)
        };

        _httpClient.DefaultRequestHeaders
            .Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
        
    
    public async Task Run()
    {
        Console.WriteLine("Welcome to the Issue Tracker App!");
        Console.WriteLine("What do you want to do?");

        bool isQuit;
        do
        {
            PrintMenu();

            string? opt = Console.ReadLine();

            isQuit = await ProcessOption(opt);
        } while (!isQuit);
    }

    private void PrintMenu()
    {
        Console.WriteLine("1 - Create a new issue.");
        Console.WriteLine("2 - Update a new issue.");
        Console.WriteLine("3 - Get an issue.");
        Console.WriteLine("4 - Delete an issue.");
    
        Console.WriteLine("0 - Exit");
    }

    private async Task<bool> ProcessOption(string? opt)
    {
        int optInt = int.Parse(opt);
        if (optInt == 1)
        {
            _issueId = await CreateIssue();
            Console.WriteLine($"Issue created! Id{_issueId}");

        } else if (optInt == 2)
        {
            await UpdateIssue();
            Console.WriteLine("Issue updated!");

        } else if (optInt == 3)
        {
            var issue = await GetIssue();
            
            if (issue is null)
                Console.WriteLine("Issue not found");

            string json = System.Text.Json.JsonSerializer.Serialize(issue);
            
            Console.WriteLine("Issue:");
            Console.WriteLine(json);

        } else if (optInt == 4)
        {
        
        } else if (optInt == 0)
        {
            Console.WriteLine("See you!");
            return true;
        }
        else
        {
            Console.WriteLine("Invalid option!");   
        }

        return false;
    }

    private async Task<Guid> CreateIssue()
    {
        var newIssue = new IssueModel
        {
            Title = "New Issue",
            Description = "Issue description"
        };

        Console.WriteLine("Creating the issue.");
        
        var resp = await _httpClient.PostAsync("",
            new StringContent(System.Text.Json.JsonSerializer.Serialize(newIssue), Encoding.UTF8, 
                "application/json")
            );

        string id = await resp.Content.ReadAsStringAsync();
        
        return System.Text.Json.JsonSerializer.Deserialize<Guid>(id);
    }
    
    private async Task UpdateIssue()
    {
        if (_issueId is null)
        {
            Console.WriteLine("you must create the issue first.");

            return;
        }
            
        var updatedIssue = new IssueModel
        {
            Id = _issueId,
            Title = "New Title",
            Description = "Issue description, now changed"
        };

        Console.WriteLine("Updating the issue.");
        
        await _httpClient.PatchAsync("",
            new StringContent(System.Text.Json.JsonSerializer.Serialize(updatedIssue), Encoding.UTF8, 
                "application/json")
        );

    }
    
    private async Task<IssueModel?> GetIssue()
    {
        Console.WriteLine("Retrieving the issue.");
        Guid id = _issueId ?? Guid.Empty;

        var resp = await _httpClient.GetAsync($"/{id}");
        
        string issue = await resp.Content.ReadAsStringAsync();
        
        return System.Text.Json.JsonSerializer.Deserialize<IssueModel>(issue);
    }
}