using Fanex.Mattermost.Client.Models;
using Maintenance.Mattermost.Constants;
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
            TargetName = CommonConstants.Mattermost,
            Group = CommonConstants.All
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
            TargetName = CommonConstants.Mattermost,
            Group = CommonConstants.All
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
            Status = (Status)httpParams.Status
        };

        var result = await service.GetLogs(queryParams);

        return Json(result);
    }

    [HttpPost]
    public async Task<ActionResult> ReSendMessage([FromBody] HttpResendParams httpParams)
    {
        var resendParams = new QueryParams
        {
            FromDate = httpParams.FromDate,
            ToDate = httpParams.ToDate,
            Id = httpParams.Id
        };

        var result = await service.ReSendMessage(resendParams);

        return Json(result);
    }

    [HttpPost]
    public async Task<ActionResult> ReSendMail([FromBody] HttpResendParams httpParams)
    {
        var resendParams = new QueryParams
        {
            FromDate = httpParams.FromDate,
            ToDate = httpParams.ToDate,
            Id = httpParams.Id
        };

        var result = await service.ReSendMail(resendParams);

        return Json(result);
    }

    [HttpPost]
    public async Task<ActionResult> ResendMultiple([FromBody] HttpResendMultipleParams httpParams)
    {
        var result = new List<bool>();
        foreach (var item in httpParams.Data)
        {
            var resendParams = new QueryParams
            {
                FromDate = httpParams.FromDate,
                ToDate = httpParams.ToDate,
                Id = item.Id
            };
            var temp = item.IsMessage ? await service.ReSendMessage(resendParams) : await service.ReSendMail(resendParams);
            if (!temp)
            {
                result.Add(false);
            }
        }

        return Json(result);
    }
}