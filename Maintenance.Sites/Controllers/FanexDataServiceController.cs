namespace Maintenance.Sites.Controllers;

using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class ServiceHealthModel
{
    public string MachineName { get; set; } = string.Empty;
    public bool Alive { get; set; }
    public int MinutesToLive { get; set; }
    public int MinutesWorked { get; set; }
}

public class ServiceViewModel
{
    public List<string> ServiceUrls { get; set; } = [];
    public string SelectedService { get; set; } = string.Empty;
    public List<int> Requests { get; set; } = [];
    public List<int> Responses { get; set; } = [];
    public Dictionary<string, int> DbStatistic { get; set; } = new();
    public Dictionary<string, int> SpStatistic { get; set; } = new();
    public int ServiceUptime { get; set; }
    public int UptimeMinutes { get; set; }
    public int MaximumUptimeMinutes { get; set; }
    public string Server { get; set; }
}

public class ServicePostedModel
{
    public string SelectedService { get; set; } = string.Empty;
}

[AllowAnonymous]
public class FanexDataServiceController(IConfiguration configuration, IHttpClientFactory httpClientFactory) : Controller
{
    private readonly List<string> ServiceUrls =
        configuration.GetValue<string>("AllFanexDataServiceApi").Split(';').ToList();

    [HttpGet]
    public async Task<ActionResult> Index()
    {
        var selectedService = ServiceUrls.FirstOrDefault() ?? string.Empty;
        var model = await CreateServiceViewModel(selectedService);
        model.SelectedService = selectedService;
        model.ServiceUrls = ServiceUrls;
        
        return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> Index(ServicePostedModel postedModel)
    {
        var model = await CreateServiceViewModel(postedModel.SelectedService);
        model.ServiceUrls = ServiceUrls;
        model.SelectedService = postedModel.SelectedService;
        return View(model);
    }

    private async Task<ServiceViewModel> CreateServiceViewModel(string serviceUrl)
    {
        using var client = httpClientFactory.CreateClient();
        client.Timeout = TimeSpan.FromSeconds(30);
        var endpoint = $"{serviceUrl}/statistic";
        var response = await client.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<ServiceViewModel>(jsonString, options);
    }
}