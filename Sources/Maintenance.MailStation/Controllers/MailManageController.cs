using Maintenance.MailStation.Constants;
using Maintenance.MailStation.Criteria.MailGroup;
using Maintenance.MailStation.Criteria.MailManage;
using Maintenance.MailStation.Extensions;
using Maintenance.MailStation.Resources;
using Maintenance.MailStation.Services;

namespace Maintenance.MailStation.Controllers;

using System.Linq;
using Fanex.NAP.Client;
using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;
using Microsoft.AspNetCore.Mvc;
using Models;

[Area("MailStation")]
[SessionAuthorize]
public class MailManageController(INapSession napSession, IMailManageService mailManageService) : Controller
{
    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Read)]
    public ActionResult Index(GetMailListCriteria criteria)
    {
        ViewBag.ListMailGroup = mailManageService.MailGroups();
        ViewBag.ListSite = mailManageService.Sites();
        ViewBag.ListStatus = mailManageService.Status();
        ViewBag.ListTriggerType = mailManageService.Trigger();
        ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
        ViewBag.IsUseCookie = string.IsNullOrEmpty(criteria.Emails) && string.IsNullOrEmpty(criteria.MailGroupId);

        return View(criteria);
    }

    public JsonResult Sites()
    {
        return Json(mailManageService.Sites());
    }

    public JsonResult MailGroups(bool hasAllElement = true)
    {
        return Json(mailManageService.MailGroups(hasAllElement));
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Host)]
    public JsonResult AddMailGroup([FromBody] CreateMailGroupCriteria criteria)
    {
        return Json(mailManageService.CreateMailGroup(criteria));
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Read)]
    public ActionResult CreateMailView(GetMailByIdCriteria criteria)
    {
        ViewBag.ListMailGroup = mailManageService.MailGroups(false);
        ViewBag.ListSite = mailManageService.Sites(false);
        ViewBag.ListTriggerType = mailManageService.Trigger(false);

        var mailModel = mailManageService.MailById(criteria);

        if (mailModel != null)
        {
            mailModel.MailName += Labels.CopyPrefix;
        }

        ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
        var model = new CreateMailCriteria(mailModel);

        return View(model);
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Host)]
    public JsonResult CreateMail([FromBody] CreateMailCriteria criteria)
    {
        if (criteria.StartOn != null)
        {
            criteria.StartOn = criteria.StartOn.Value.ToUniversalTime();
        }

        if (criteria.EndOn != null)
        {
            criteria.EndOn = criteria.EndOn.Value.ToUniversalTime();
        }
        criteria.ModifiedBy = napSession.UserName;

        return Json(mailManageService.CreateMail(criteria));
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Read)]
    public ActionResult MailList([FromBody] GetMailListCriteria criteria)
    {
        var result = mailManageService.MailList(criteria);

        return Json(result);
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Read)]
    public ActionResult MailActionLog(int mailId)
    {
        var mailLog = mailManageService.MailLogById(new GetMailLogByIdCriteria { MailId = mailId });
        ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();

        return View(mailLog.ToList());
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Host)]
    public ActionResult DeleteMail([FromBody] DeleteMailCriteria criteria)
    {
        var commandResult = mailManageService.DeleteMail(criteria);

        return Json(commandResult);
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Read)]
    public ActionResult UpdateMailView(GetMailByIdCriteria criteria)
    {
        ViewBag.ListMailGroup = mailManageService.MailGroups(false);
        ViewBag.ListSite = mailManageService.Sites(false);
        ViewBag.ListTriggerType = mailManageService.Trigger(false);
        ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();

        var mailModel = mailManageService.MailById(criteria);
        var model = new UpdateMailCriteria(mailModel);

        return View(model);
    }

    [HttpPost]
    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Host)]
    public JsonResult UpdateMail([FromBody] UpdateMailCriteria criteria)
    {
        criteria.ModifiedBy = napSession.UserName;

        var commandResult = mailManageService.UpdateMail(criteria);

        return Json(commandResult);
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Read)]
    public JsonResult SuggestMail(SuggestMailCriteria criteria)
    {
        return Json(mailManageService.SuggestMails(criteria));
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Host)]
    public void RunNow([FromBody] ParamModel paramModel)
    {
        mailManageService.RunNow(paramModel.MailId);
    }
}