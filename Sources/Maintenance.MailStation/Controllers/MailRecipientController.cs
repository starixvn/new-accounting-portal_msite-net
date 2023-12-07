using Maintenance.MailStation.Constants;
using Maintenance.MailStation.Criteria.ManageRecipient;
using Maintenance.MailStation.Extensions;
using Maintenance.MailStation.Models;

namespace Maintenance.MailStation.Controllers;

using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;
using Microsoft.AspNetCore.Mvc;
using Services;

[NapSuperAuth]
public class MailRecipientController : Controller
{
    private readonly ManageRecipientService manageRecipientService = new();

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageRecipient, Policy = Constant.Policy.Read)]
    public ActionResult Index()
    {
        ViewBag.ListStatus = manageRecipientService.Status();
        ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();

        return View("~/Areas/MailStation/ManageRecipient/ManageRecipient/Index.cshtml");
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageRecipient, Policy = Constant.Policy.Host)]
    public ActionResult CreateRecipientView()
    {
        ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();

        return View("~/Areas/MailStation/ManageRecipient/CreateRecipient/Index.cshtml");
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageRecipient, Policy = Constant.Policy.Read)]
    public ActionResult RecipientList(RecipientListCriteria criteria)
    {
        var recipients = manageRecipientService.RecipientList(criteria);

        return Json(recipients);
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageRecipient, Policy = Constant.Policy.Host)]
    public ActionResult UpdateRecipientView(RecipientByIdCriteria criteria)
    {
        var recipient = manageRecipientService.RecipientById(criteria);
        var recipientModel = new RecipientModel
        {
            RecipientId = recipient.RecipientId,
            RecipientName = recipient.RecipientName,

            EmailId1 = recipient.EmailId1,
            EmailId2 = recipient.EmailId2,
            EmailId3 = recipient.EmailId3,
            Email1 = recipient.Email1,
            Email2 = recipient.Email2,
            Email3 = recipient.Email3,

            IsNapUser = recipient.IsNapUser,
            NapUserId = recipient.NapUserId,

            IsActive = recipient.IsActive,
            MailRelevant = recipient.MailRelevant,
            Note = recipient.Note
        };

        ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();

        return View("~/Areas/MailStation/ManageRecipient/UpdateRecipient/Index.cshtml", recipientModel);
    }

    [HttpPost]
    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageRecipient, Policy = Constant.Policy.Host)]
    public JsonResult UpdateRecipient(UpdateRecipientCriteria criteria)
    {
        var commandResult = manageRecipientService.UpdateRecipient(criteria);

        return Json(commandResult);
    }

    [HttpPost]
    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageRecipient, Policy = Constant.Policy.Host)]
    public JsonResult DeleteRecipient(DeleteRecipientCriteria criteria)
    {
        var commandResult = manageRecipientService.DeleteRecipient(criteria);

        return Json(commandResult);
    }

    [HttpPost]
    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageRecipient, Policy = Constant.Policy.Host)]
    public JsonResult CreateRecipient(CreateRecipientCriteria criteria)
    {
        var commandResult = manageRecipientService.CreateRecipient(criteria);

        return Json(commandResult);
    }

    [HttpPost]
    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageRecipient, Policy = Constant.Policy.Read)]
    public JsonResult CheckRecipient(CheckRecipientInfoCriteria criteria)
    {
        return Json(manageRecipientService.CheckRecipient(criteria));
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageRecipient, Policy = Constant.Policy.Read)]
    public ActionResult ViewRelevantMail(RecipientByIdCriteria criteria)
    {
        var recipient = manageRecipientService.RecipientById(criteria);
        var emails = new[] { recipient.Email1, recipient.Email2, recipient.Email3 };

        return RedirectToAction("Index", "MailManage", new { Emails = string.Join(",", emails).Trim(',') });
    }
}