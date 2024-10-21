using Maintenance.MailStation.Constants;
using Maintenance.MailStation.Criteria.MailReport;
using Maintenance.MailStation.Extensions;
using Maintenance.MailStation.Services;

namespace Maintenance.MailStation.Controllers;

using Fanex.MailDelivery.Core.Models;
using Fanex.NAP.Client;
using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;
using Microsoft.AspNetCore.Mvc;
using Models;

[Area("MailStation")]
[SessionAuthorize]
public class MailReportController(INapSession napSession, IMailReportService mailReportService) : Controller
{
    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Read)]
    public ActionResult Index()
    {
        ViewBag.ListStatus = mailReportService.TaskStatus();
        ViewBag.ListSite = mailReportService.Sites();
        ViewBag.ListTriggerType = mailReportService.Trigger();
        ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
        ViewBag.DateTime = DateTime.Now.ToString("MMM dd,yyyy");

        return View();
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Read)]
    [HttpPost]
    public ActionResult MailReport([FromBody] MailReportCriteria criteria)
    {
        return Json(mailReportService.MailReport(criteria));
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Read)]
    public ActionResult GetMailTaskStatusCounter()
    {
        return Json(mailReportService.GetMailTaskStatusCounter());
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Read)]
    public ActionResult MailDetail(int mailId)
    {
        var mailDetail = mailReportService.MailDetail(new MailDetailCriteria { MailId = mailId });
        ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();

        return View(mailDetail);
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Read)]
    public ActionResult MailLog(int mailId)
    {
        var mailLog = mailReportService.MailLog(new MailLogCriteria { MailId = mailId });
        ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();

        return View(mailLog);
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Host)]
    public void AbortTask([FromBody] ParamModel paramModel)
    {
        var taskId = paramModel.TaskId;
        mailReportService.AbortTask(taskId);

        mailReportService.AddMailLog(new AddMailLogCriteria
        {
            ExceptionMessage = $"> It's aborted by \"{napSession.UserName}\"",
            LogTime = DateTime.Now,
            Status = (int)JobStatus.Abort,
            TaskId = taskId
        });
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Read)]
    public ActionResult RetrySendMail([FromBody] RetrySendMailCriteria criteria)
    {
        mailReportService.AddMailLog(new AddMailLogCriteria
        {
            ExceptionMessage = $"> It's retried by \"{napSession.UserName}\"",
            LogTime = DateTime.Now,
            Status = (int)JobStatus.Running,
            TaskId = criteria.TaskId
        });

        mailReportService.RetrySendMail(criteria);

        return new EmptyResult();
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Host)]
    public ActionResult RetryMissingTask(RetryMissingTaskCriteria criteria)
    {
        mailReportService.RetryMissingJob(criteria);

        return new EmptyResult();
    }
}