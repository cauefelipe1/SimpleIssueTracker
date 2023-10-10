using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using IssueTrackerModels.Models;

namespace IssueTrackerApiClient;

public class IssueTrackerClient
{
    private readonly string _baseUrl = "http://localhost:5274";
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

        } else if (optInt == 2)
        {
            await UpdateIssue();

        } else if (optInt == 3)
        {
            var issue = await GetIssue();
            
            if (issue is null)
                Console.WriteLine("Issue not found");

            string json = JsonSerializer.Serialize(issue);
            
            Console.WriteLine("Issue:");
            Console.WriteLine(json);

        } else if (optInt == 4)
        {
            await DeleteIssue();
            
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
        
        var resp = await _httpClient.PostAsync("issue",
            new StringContent(System.Text.Json.JsonSerializer.Serialize(newIssue), Encoding.UTF8, 
                "application/json")
            );

        string id = await resp.Content.ReadAsStringAsync();
        
        Console.WriteLine($"Issue created! Id: {_issueId}");

        return JsonSerializer.Deserialize<Guid>(id);
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
        
        await _httpClient.PutAsync("issue",
            new StringContent(JsonSerializer.Serialize(updatedIssue), Encoding.UTF8, 
                "application/json")
        );
        
        Console.WriteLine("Issue updated!");
    }
    
    private async Task<IssueModel?> GetIssue()
    {
        Console.WriteLine("Retrieving the issue.");
        Guid id = _issueId ?? Guid.Empty;

        var resp = await _httpClient.GetAsync($"issue/{id}");

        if (resp.StatusCode == HttpStatusCode.NotFound)
            return null;
        
        string issue = await resp.Content.ReadAsStringAsync();

        var jsonOpt = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        
        return JsonSerializer.Deserialize<IssueModel>(issue, jsonOpt);
    }
    
    private async Task DeleteIssue()
    {
        if (_issueId is null)
        {
            Console.WriteLine("you must create the issue first.");

            return;
        }
        
        Console.WriteLine("Deleting the issue.");

        await _httpClient.DeleteAsync($"issue/{_issueId}");

        _issueId = null;
        
        Console.WriteLine("Issue deleted!");
    }
}