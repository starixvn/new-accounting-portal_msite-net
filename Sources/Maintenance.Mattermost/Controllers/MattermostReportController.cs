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
    public ActionResult GetTargets([FromBody] HttpGetLogParams httpParams)
    {
        var queryParams = new GetLogParams
        {
            FromDate = httpParams.FromDate,
            ToDate = httpParams.ToDate,
            Id = 0
        };

        var result = service.GetTargets(queryParams);
        return Json(result);
    }

    [HttpPost]
    public ActionResult GetGroupsByTarget([FromBody] HttpGetLogParams httpParams)
    {
        var queryParams = new GetLogParams
        {
            FromDate = httpParams.FromDate,
            ToDate = httpParams.ToDate,
            TargetName = httpParams.TargetName,
            Id = 0
        };

        var result = service.GetGroupsByTarget(queryParams);
        return Json(result);
    }

    [HttpPost]
    public ActionResult GetReport([FromBody] HttpGetLogParams httpParams)
    {
        var queryParams = new GetLogParams
        {
            FromDate = httpParams.FromDate,
            ToDate = httpParams.ToDate.AddDays(1),
            TargetName = httpParams.TargetName,
            Group = httpParams.Group,
            Id = 0,
            Status = httpParams.Status
        };

        var result = service.GetLogs(queryParams);

        return Json(result);
    }
}