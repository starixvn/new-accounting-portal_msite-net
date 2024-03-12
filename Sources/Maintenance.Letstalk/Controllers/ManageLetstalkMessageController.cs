namespace Maintenance.Letstalk.Controllers;

using System.Collections.Generic;
using System.Linq;
using Criteria;
using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Business;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Client.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services;

[NapSuperAuth]
public class ManageLetstalkMessageController(
    INapSession napSession,
    INapSettings napSettings,
    IUserService userService,
    IManageLetstalkMessageService manageLetstalkMessageService) : Controller
{
    [NapAuth(FunctionKey = SiteFunctionKeys.ManageLetstalkMessage, Policy = Constant.Policy.Read)]
    public ActionResult Index()
    {
        ViewBag.HasEdit = HasRight(SiteFunctionKeys.ManageLetstalkMessage, Constant.Policy.Read);
        ViewBag.StatusList = manageLetstalkMessageService.GetStatusList();
        ViewBag.LetstalkGroupList = manageLetstalkMessageService.GetGroups();
        return View();
    }

    [NapAuth(FunctionKey = SiteFunctionKeys.ManageLetstalkMessage, Policy = Constant.Policy.Read)]
    public ActionResult CreateLetstalkMessage()
    {
        ViewBag.LetstalkGroupList = manageLetstalkMessageService.GetGroups();
        return View();
    }

    [NapAuth(FunctionKey = SiteFunctionKeys.ManageLetstalkMessage, Policy = Constant.Policy.Read)]
    public ActionResult EditLetstalkMessage(int notificationID, int recipientID)
    {
        var criteria = new GetNotificationListCriteria
        {
            RecipientID = recipientID,
            NotificationId = notificationID
        };
        ViewBag.LetstalkGroupList = manageLetstalkMessageService.GetGroups();
        var model = manageLetstalkMessageService.GetNotificationById(criteria).FirstOrDefault();
        return View(model);
    }

    [NapAuth(FunctionKey = SiteFunctionKeys.ManageLetstalkMessage, Policy = Constant.Policy.Read)]
    [HttpPost]
    public JsonResult InsertNotification(IFormCollection formValues, IEnumerable<IFormFile> files)
    {
        var insertNotificationCriteria = JsonConvert.DeserializeObject<InsertNotificationCriteria>(formValues["insertNotificationCriteria"]);
        insertNotificationCriteria.CreatedBy = napSession.UserId;
        var result = manageLetstalkMessageService.InsertNotification(insertNotificationCriteria, files);
        return Json(result);
    }

    [NapAuth(FunctionKey = SiteFunctionKeys.ManageLetstalkMessage, Policy = Constant.Policy.Read)]
    [HttpPost]
    public JsonResult EditNotification(IFormCollection formValues, IEnumerable<IFormFile> files)
    {
        var editNotificationCriteria = JsonConvert.DeserializeObject<EditNotificationCriteria>(formValues["editNotificationCriteria"]);
        editNotificationCriteria.LastModifiedBy = napSession.UserId;
        var result = manageLetstalkMessageService.EditNotification(editNotificationCriteria, files);
        return Json(result);
    }

    [NapAuth(FunctionKey = SiteFunctionKeys.ManageLetstalkMessage, Policy = Constant.Policy.Read)]
    [HttpPost]
    public JsonResult DeleteNotification([FromBody] DeleteNotificationCriteria criteria)
    {
        criteria.LastModifiedBy = napSession.UserId;
        var result = manageLetstalkMessageService.DeleteNotification(criteria);
        return Json(result);
    }

    [NapAuth(FunctionKey = SiteFunctionKeys.ManageLetstalkMessage, Policy = Constant.Policy.Read)]
    [HttpPost]
    public JsonResult ResendNotification([FromBody] ResendNotificationCriteria criteria)
    {
        var result = manageLetstalkMessageService.ResendNotification(criteria);
        return Json(result);
    }

    [HttpPost]
    public JsonResult GetGroups()
    {
        var result = manageLetstalkMessageService.GetGroups();
        return Json(result);
    }

    [NapAuth(FunctionKey = SiteFunctionKeys.ManageLetstalkMessage, Policy = Constant.Policy.Read)]
    [HttpPost]
    public JsonResult GetNotificationList([FromBody] GetNotificationListCriteria criteria)
    {
        var result = manageLetstalkMessageService.GetNotificationList(criteria);
        return Json(result);
    }

    public bool HasRight(string functionKey, Constant.Policy policy)
    {
        var functionPolicy = userService.GetPermission(napSession.UserId, napSettings.SiteId, functionKey);

        return functionPolicy >= (int)policy;
    }
}