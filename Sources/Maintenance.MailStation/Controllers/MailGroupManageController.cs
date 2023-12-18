using Maintenance.MailStation.Constants;
using Maintenance.MailStation.Criteria.MailGroup;
using Maintenance.MailStation.Extensions;
using Maintenance.MailStation.Services;

namespace Maintenance.MailStation.Controllers;

using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;
using Microsoft.AspNetCore.Mvc;

[NapSuperAuth]
public class MailGroupManageController(IMailGroupService mailGroupService) : Controller
{
    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMailGroup, Policy = Constant.Policy.Read)]
    public ActionResult Index(GetMailGroupCriteria criteria)
    {
        ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
        return View("~/Areas/MailStation/MailGroup/ManageMailGroup/Index.cshtml", criteria);
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMailGroup, Policy = Constant.Policy.Host)]
    public ActionResult CreateMailGroupView()
    {
        ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
        return View("~/Areas/MailStation/MailGroup/CreateMailGroup/Index.cshtml");
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMailGroup, Policy = Constant.Policy.Host)]
    public JsonResult CreateMailGroup(CreateMailGroupCriteria criteria)
    {
        var commandResult = mailGroupService.CreateMailGroup(criteria);

        return Json(commandResult);
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMailGroup, Policy = Constant.Policy.Read)]
    public JsonResult MailGroupList(GetMailGroupCriteria criteria)
    {
        var groups = mailGroupService.GetMailGroups(criteria);

        return Json(groups);
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMailGroup, Policy = Constant.Policy.Host)]
    public ActionResult DeleteMailGroup(DeleteMailGroupCriteria criteria)
    {
        var commandResult = mailGroupService.DeleteMailGroup(criteria);

        return Json(commandResult);
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMailGroup, Policy = Constant.Policy.Host)]
    public ActionResult UpdateMailGroupView(GetMailGroupByIdCriteria criteria)
    {
        var model = new UpdateMailGroupCriteria();
        var group = mailGroupService.GetMailGroupById(criteria);

        if (group != null)
        {
            model.MailGroupId = group.MailGroupId;
            model.MailGroupName = group.MailGroupName;
            model.Description = group.Description;
        }

        ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();

        return View("~/Areas/MailStation/MailGroup/UpdateMailGroup/Index.cshtml", model);
    }

    [HttpPost]
    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMailGroup, Policy = Constant.Policy.Host)]
    public JsonResult UpdateMailGroup(UpdateMailGroupCriteria criteria)
    {
        var commandResult = mailGroupService.UpdateMailGroup(criteria);

        return Json(commandResult);
    }

    [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMailGroup, Policy = Constant.Policy.Read)]
    public ActionResult ViewRelevantMail(int mailGroupId)
    {
        return RedirectToAction("Index", "MailManage", new { MailGroupId = mailGroupId });
    }
}