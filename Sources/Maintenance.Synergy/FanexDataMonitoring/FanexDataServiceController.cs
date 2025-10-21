using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Maintenance.Synergy.FanexDataMonitoring;

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
        configuration.GetSection("AllFanexDataServiceApis").Get<List<string>>();

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
        using var client = CreateHttpClient();
        var endpoint = $"{serviceUrl}/statistic";
        var response = await client.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var viewModel = JsonSerializer.Deserialize<ServiceViewModel>(jsonString, options);
        viewModel.Responses = TrimZeroRight(viewModel.Responses);

        return viewModel;
    }

    [HttpGet]
    public async Task<ActionResult> Gc(string serviceUrl)
    {
        using var client = CreateHttpClient();
        var endpoint = $"{serviceUrl}/gc";
        var response = await client.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        var gcResult = await response.Content.ReadAsStringAsync();
        
        return Content(gcResult);
    }

    [HttpGet]
    public async Task<ActionResult> Kill(string serviceUrl)
    {
        using var client = CreateHttpClient();
        var endpoint = $"{serviceUrl}/kill";
        var response = await client.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        var killResult = await response.Content.ReadAsStringAsync();
        
        return Content(killResult);
    }

    [HttpGet]
    public async Task<ActionResult> Clear(string serviceUrl)
    {
        using var client = CreateHttpClient();
        var endpoint = $"{serviceUrl}/api/cache/clean";
        var response = await client.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        var clearResult = await response.Content.ReadAsStringAsync();

        return Content($"Fanex Data cache of {clearResult} was cleared");
    }

    [HttpGet]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<ActionResult> Status(string serviceUrl)
    {
        using var client = CreateHttpClient();
        var endpoint = $"{serviceUrl}";
        var response = await client.GetAsync(endpoint);
        var statusResult = await response.Content.ReadAsStringAsync();

        return Content($"StatusCode: {response.StatusCode}. {statusResult}");
    }

    private HttpClient CreateHttpClient()
    {
        var client = httpClientFactory.CreateClient();
        client.Timeout = TimeSpan.FromSeconds(30);
        client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
        {
            NoCache = true,
            NoStore = true
        };
        client.DefaultRequestHeaders.Add("Pragma", "no-cache");
        return client;
    }

    /// <summary>
    /// Removes trailing zeros from a list of integers until encountering the first non-zero number from the right.
    /// </summary>
    /// <param name="input">The list of integers to process</param>
    /// <returns>A new list with trailing zeros removed</returns>
    private static List<int> TrimZeroRight(List<int> input)
    {
        if (input == null || input.Count == 0)
        {
            return [];
        }

        var lastNonZeroIndex = input.Count - 1;
        while (lastNonZeroIndex >= 0 && input[lastNonZeroIndex] == 0)
        {
            lastNonZeroIndex--;
        }

        return input.Take(lastNonZeroIndex + 1).ToList();
    }
}