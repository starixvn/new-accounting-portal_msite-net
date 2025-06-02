using Fanex.Mattermost.Client.Models;
using Maintenance.Mattermost.Params;
using Maintenance.Mattermost.Services;
using Microsoft.AspNetCore.Mvc;

namespace Maintenance.Mattermost.Controllers;

public class MattermostReportController(MattermostReportRestClientService service) : Controller
{
    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> GetTargets([FromBody] HttpGetLogParams httpParams)
    {
        var queryParams = new QueryParams
        {
            FromDate = httpParams.FromDate,
            ToDate = httpParams.ToDate,
            Id = 0
        };

        var result = await service.GetTargets(queryParams);
        return Json(result);
    }

    [HttpPost]
    public async Task<ActionResult> GetGroupsByTarget([FromBody] HttpGetLogParams httpParams)
    {
        var queryParams = new QueryParams
        {
            FromDate = httpParams.FromDate,
            ToDate = httpParams.ToDate,
            TargetName = httpParams.TargetName,
            Id = 0
        };

        var result = await service.GetGroupsByTarget(queryParams);
        return Json(result);
    }

    [HttpPost]
    public async Task<ActionResult> GetReport([FromBody] HttpGetLogParams httpParams)
    {
        var queryParams = new QueryParams
        {
            FromDate = httpParams.FromDate,
            ToDate = httpParams.ToDate.AddDays(1),
            TargetName = httpParams.TargetName,
            Group = httpParams.Group,
            Id = 0,
            Status = (Status)httpParams.Status
        };

        var result = await service.GetLogs(queryParams);

        return Json(result);
    }
}