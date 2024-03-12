namespace Maintenance.Letstalk.Controllers;

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Params;
using Services;

public class LetstalkReportController(ILetstalkReportRestClientService service) : Controller
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

    [HttpPost]
    public ActionResult ReSendMessage([FromBody] HttpResendParams httpParams)
    {
        var resendParams = new ResendParams
        {
            FromDate = httpParams.FromDate,
            ToDate = httpParams.ToDate,
            Id = httpParams.Id
        };

        var result = service.ReSendMessage(resendParams);

        return Json(result);
    }

    [HttpPost]
    public ActionResult ReSendMail([FromBody] HttpResendParams httpParams)
    {
        var resendParams = new ResendParams
        {
            FromDate = httpParams.FromDate,
            ToDate = httpParams.ToDate,
            Id = httpParams.Id
        };

        var result = service.ReSendMail(resendParams);

        return Json(result);
    }

    [HttpPost]
    public ActionResult ResendMultiple([FromBody] HttpResendMultipleParams httpParams)
    {
        var result = new List<bool>();
        foreach (var item in httpParams.Data)
        {
            var resendParams = new ResendParams
            {
                FromDate = httpParams.FromDate,
                ToDate = httpParams.ToDate,
                Id = item.Id
            };
            var temp = item.IsMessage ? service.ReSendMessage(resendParams) : service.ReSendMail(resendParams);
            if (!temp)
            {
                result.Add(false);
            }
        }

        return Json(result);
    }

}