using Fanex.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Maintenance.Sites.Controllers;

public class UmSportModel
{
    [Required]
    public string SportTypeIds { get; set; }
}

public class UmSportRequest
{
    public string Timestamp { get; set; }

    public string Seq { get; set; }

    public int SportTypeId { get; set; }
}

public class ResultModel
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public string Seq { get; set; }

    public int StatusCode { get; set; }

    public int ErrorCode { get; set; }

    public string ErrorDescription { get; set; }
}

[AllowAnonymous]
public class UmSportController(IConfiguration configuration) : Controller
{
    private static readonly HttpClient httpClient = new()
    {
        DefaultRequestHeaders = { { "User-Agent", "MSite.UMSport" } }
    };

    public IActionResult Index()
    {
        if (configuration.GetSection("UmSportApi").Value == "")
        {
            return NotFound();
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Execute(string submitType, UmSportModel request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "SportTypeIds is required!!!";
                return RedirectToAction("Index");
            }

            var endpoint = submitType switch
            {
                "Set UM" => "Set3RdPartyUm",
                "Remove UM" => "Remove3rdPartyUM",
                _ => "Get3RdPartyUm"
            };
            var result = new List<ResultModel>();
            var ids = request.SportTypeIds.Trim().Split(",");
            foreach (var id in ids)
            {
                var umRequest = new UmSportRequest
                {
                    Timestamp = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff tt}",
                    SportTypeId = int.Parse(id),
                    Seq = id
                };
                var response = await SendUmRequest(umRequest, endpoint);
                result.Add(response);
            }

            TempData["Error"] = null;
            TempData["Result"] = JsonConvert.SerializeObject(result);
            await LogService.InfoAsync($"Action: {endpoint}, SportTypeIds: {request.SportTypeIds}, Result: {JsonConvert.SerializeObject(result)}, Time: {DateTime.Now}");
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

    private async Task<ResultModel> SendUmRequest(UmSportRequest umRequest, string endpoint)
    {
        try
        {
            httpClient.DefaultRequestHeaders.Remove("Authentication");
            httpClient.DefaultRequestHeaders.Add("Authentication", configuration.GetSection("UmSportApiKey").Value);

            var jsonContent = JsonConvert.SerializeObject(umRequest, Formatting.Indented);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var url = $"{configuration.GetSection("UmSportApi").Value}/{endpoint}";
            var response = await httpClient.PostAsync(url, content);

            response.EnsureSuccessStatusCode();
            var statusCode = (int)response.StatusCode;

            var stringResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResultModel>(stringResult) ?? new ResultModel();
            result.StatusCode = statusCode;

            return result;
        }
        catch (HttpRequestException ex)
        {
            return new ResultModel
            {
                ErrorCode = 1,
                ErrorDescription = $"HTTP request failed: {ex.Message}"
            };
        }
        catch (JsonException ex)
        {
            return new ResultModel
            {
                ErrorCode = 1,
                ErrorDescription = $"JSON serialization/deserialization failed: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            return new ResultModel
            {
                ErrorCode = 1,
                ErrorDescription = $"Unexpected error: {ex.Message}"
            };
        }
    }
}