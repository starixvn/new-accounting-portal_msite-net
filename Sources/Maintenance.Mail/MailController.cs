namespace Maintenance.Mail;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Criteria.MailService;
using Fanex.MailDeliver.MailClient;
using Fanex.MailDeliver.MailClient.Model;
using Fanex.NAP.Client;
using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Business;
using Fanex.NAP.Client.Services;
using Maintenance.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Services;

[SessionAuthorize]
public class MailController(
    IMailService mailServer,
    IUserService userService,
    IMailRepository mailRepository,
    INapSession napSession,
    INapSettings settings,
    IConfiguration configuration) : Controller
{
    #region Action

    #region MailList

    [HttpGet]
    [NapAuth(FunctionKey = SiteFunctionKeys.ViewMailReport, Policy = Constant.Policy.Read)]
    public ActionResult MailList(int appId = 0)
    {
        InitPageSize();
        InitDataMailConfig(appId);
        InitMailStatus();

        var totalRows = 0;

        ViewBag.DateFrom = DateTime.Now.ToString("dd/MM/yyyy");
        ViewBag.DateTo = DateTime.Now.ToString("dd/MM/yyyy");

        var listMail = mailServer.WithApp(appId)
            .FromDate(DateTime.Now.ToShortDateString())
            .ToDate(DateTime.Now.ToShortDateString())
            .WithStatus(0)
            .WithPagination(1, 10)
            .GetListMailByDate(ref totalRows);

        return View(listMail);
    }

    [HttpPost]
    [NapAuth(FunctionKey = SiteFunctionKeys.ViewMailReport, Policy = Constant.Policy.Read)]
    public ActionResult MailList(string from, string to, int applicationID = 0, int mailStatus = 0, int size = 10)
    {
        InitPageSize();

        InitDataMailConfig();

        InitMailStatus();

        var totalRows = 0;

        var dateFrom = DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        var dateTo = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        var listMail = mailServer.WithApp(applicationID)
            .FromDate(dateFrom.ToString(CultureInfo.CurrentCulture))
            .ToDate(dateTo.ToString(CultureInfo.CurrentCulture))
            .WithStatus(mailStatus)
            .WithPagination(1, size)
            .GetListMailByDate(ref totalRows);

        ViewBag.DateFrom = from;
        ViewBag.DateTo = to;

        return View(listMail);
    }

    #endregion MailList

    #region MailSetting

    [HttpGet]
    [NapAuth(FunctionKey = SiteFunctionKeys.SmtpServers, Policy = Constant.Policy.Read)]
    public ActionResult MailSetting()
    {
        var listMailSetting = mailServer.GetListSetting();
        ViewBag.HasEdit = HasRight(SiteFunctionKeys.SmtpServers, Constant.Policy.Host);

        return View(listMailSetting);
    }

    [HttpGet]
    [NapAuth(FunctionKey = SiteFunctionKeys.SmtpServers, Policy = Constant.Policy.Host)]
    public ActionResult AddMailSetting()
    {
        return View(new MailSettingViewModel());
    }

    [HttpPost]
    [NapAuth(FunctionKey = SiteFunctionKeys.SmtpServers, Policy = Constant.Policy.Host)]
    public ActionResult AddMailSetting(MailSettingViewModel model)
    {
        ViewBag.Success = false;

        if (ModelState.IsValid)
        {
            model.Password ??= string.Empty;
            model.SettingName = model.Server + ":" + model.Port + " (" + model.Username + ")";
            var mailSetting = model.GetMailSetting();

            ViewBag.Success = mailServer.StoreSetting(mailSetting);
        }

        return View(model);
    }

    [HttpGet]
    public ActionResult AddMailSettingPopUp()
    {
        return View(new MailSetting());
    }

    [HttpGet]
    [NapAuth(FunctionKey = SiteFunctionKeys.SmtpServers, Policy = Constant.Policy.Host)]
    public ActionResult EditMailSetting(int id)
    {
        var mailSetting = mailServer.GetSetting(id);
        var model = new MailSettingViewModel(mailSetting);

        return View(model);
    }

    [HttpPost]
    [NapAuth(FunctionKey = SiteFunctionKeys.SmtpServers, Policy = Constant.Policy.Host)]
    public ActionResult EditMailSetting(MailSettingViewModel model)
    {
        ViewBag.Success = false;

        if (ModelState.IsValid)
        {
            model.Password ??= string.Empty;
            model.SettingName = model.Server + ":" + model.Port + " (" + model.Username + ")";
            var mailSetting = model.GetMailSetting();
            ViewBag.Success = mailServer.StoreSetting(mailSetting);
        }

        return View(model);
    }

    #endregion MailSetting

    #region MailConfig

    [HttpGet]
    [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Read)]
    public ActionResult MailConfig(int id = 0)
    {
        var mailSetting = mailServer.GetSetting(id);
        var serverIp = mailSetting != null ? mailSetting.Server : string.Empty;
        InitDataIpServer(serverIp);

        var listMailConfig = mailServer.GetListConfig(serverIp);

        ViewBag.HasEdit = HasRight(SiteFunctionKeys.ApplicationManagement, Constant.Policy.Host);

        return View(listMailConfig);
    }

    [HttpPost]
    [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Read)]
    public ActionResult MailConfig(string serverIp = "")
    {
        InitDataIpServer(serverIp);

        var listMailConfig = mailServer.GetListConfig(serverIp);

        ViewBag.HasEdit = HasRight(SiteFunctionKeys.ApplicationManagement, Constant.Policy.Host);

        return View(listMailConfig);
    }

    [HttpGet]
    [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Host)]
    public ActionResult AddMailConfig()
    {
        InitDataMailSetting();

        return View(new MailConfigViewModel());
    }

    [HttpPost]
    [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Host)]
    public ActionResult AddMailConfig(MailConfigViewModel model)
    {
        InitDataMailSetting();

        ViewBag.Success = false;

        if (ModelState.IsValid)
        {
            var mailConfig = model.GetMailConfig(napSession.UserId);

            ViewBag.Success = mailServer.StoreConfig(mailConfig);
        }

        return View(model);
    }

    [HttpGet]
    public ActionResult AddMailConfigPopUp()
    {
        InitDataMailSetting();

        return View(new MailConfig());
    }

    [HttpGet]
    [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Host)]
    public ActionResult EditMailConfig(int id)
    {
        InitDataMailSetting();

        ViewBag.Success = false;

        var mailConfig = mailServer.GetConfig(id);
        var model = new MailConfigViewModel(mailConfig);

        return View(model);
    }

    [HttpPost]
    [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Host)]
    public ActionResult EditMailConfig(MailConfigViewModel model)
    {
        InitDataMailSetting();

        ViewBag.Success = false;

        if (ModelState.IsValid)
        {
            var mailConfig = model.GetMailConfig(napSession.UserId);

            ViewBag.Success = mailServer.StoreConfig(mailConfig);
        }

        return View(model);
    }

    [HttpGet]
    [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Read)]
    public ActionResult GetMailConfigLogs(int appId)
    {
        var listMailConfig = mailRepository.GetMailConfigLogs(appId);

        return View(listMailConfig);
    }

    #endregion MailConfig

    #region MailFunction

    [HttpGet]
    public ActionResult MailFunction()
    {
        var listMailFunction = mailServer.GetListFunction();

        return View(listMailFunction);
    }

    [HttpGet]
    public ActionResult AddMailFunction()
    {
        InitDataMailSetting();
        InitDataMailConfig();

        return View(new MailFunctionViewModel());
    }

    [HttpPost]
    public ActionResult AddMailFunction(MailFunctionViewModel model)
    {
        InitDataMailSetting();
        InitDataMailConfig();

        ViewBag.Success = false;

        if (ModelState.IsValid)
        {
            var mailFunction = model.GetMailFunction();

            ViewBag.Success = mailServer.StoreFunction(mailFunction);
        }

        return View(model);
    }

    [HttpGet]
    public ActionResult EditMailFunction(int id)
    {
        InitDataMailSetting();
        InitDataMailConfig();

        var mailFunction = mailServer.GetFunction(id);

        var model = new MailFunctionViewModel(mailFunction);

        return View(model);
    }

    [HttpPost]
    public ActionResult EditMailFunction(MailFunctionViewModel model)
    {
        InitDataMailSetting();
        InitDataMailConfig();

        ViewBag.Success = false;

        if (ModelState.IsValid)
        {
            var mailFunction = model.GetMailFunction();

            ViewBag.Success = mailServer.StoreFunction(mailFunction);
        }

        return View(model);
    }

    #endregion MailFunction

    #region Compose Email

    [HttpGet]
    [NapAuth(FunctionKey = SiteFunctionKeys.ComposeEmail, Policy = Constant.Policy.Read)]
    public ActionResult ComposeEmail()
    {
        InitDataMailConfig();
        return View(new MailRecord());
    }

    [HttpPost]
    [NapAuth(FunctionKey = SiteFunctionKeys.ComposeEmail, Policy = Constant.Policy.Read)]
    public ActionResult ComposeEmail(MailRecord mailRecord)
    {
        InitDataMailConfig();

        AppSettings.AppMailId = mailRecord.AppID.ToString();

        if (!string.IsNullOrWhiteSpace(mailRecord.FileAttachments))
        {
            var extension = Path.GetExtension(mailRecord.FileAttachments);

            var fileExtension = extension.ToLower() switch
            {
                ".xls" => FileExtentions.MS_EXCEL,
                ".doc" => FileExtentions.MS_DOC,
                ".jpg" => FileExtentions.IMG_JPG,
                ".gif" => FileExtentions.IMG_GIF,
                ".png" => FileExtentions.IMG_PNG,
                ".pdf" => FileExtentions.PDF,
                _ => FileExtentions.TEXT
            };

            mailRecord.Attachments = [];
            var attachment = new MailAttachment(mailRecord.FileAttachments, fileExtension);
            mailRecord.Attachments.Add(attachment);
        }

        using (var client = new Fanex.MailDeliver.MailClient.API.MailDeliver())
        {
            client.SendMail(mailRecord);
        }

        ViewBag.Success = true;

        return View(mailRecord);
    }

    #endregion Compose Email

    #endregion Action

    #region Ajax

    [HttpPost]
    public ActionResult AddMailSettingPopUp(
        string server,
        int port,
        string username,
        string password,
        bool isAuthor)
    {
        var mail = new MailSetting
        {
            SettingName = server + ":" + port + " (" + username + ") ",
            Server = server,
            Port = port,
            Username = username,
            Password = password,
            IsAuth = isAuthor
        };

        mailServer.StoreSetting(mail);
        return Json(mail);
    }

    [HttpPost]
    public ActionResult AddMailConfigPopUp(string appName, string description, int settingId)
    {
        var mailConfig = new MailConfig
        {
            AppName = appName,
            Description = description,
            Setting = new MailSetting
            {
                SettingID = settingId
            }
        };

        InitDataMailSetting();

        ViewBag.Success = false;

        if (ModelState.IsValid)
        {
            ViewBag.Success = mailServer.StoreConfig(mailConfig);
        }

        return Json(mailConfig);
    }

    [HttpPost]
    [NapAuth(FunctionKey = SiteFunctionKeys.ResendMail, Policy = Constant.Policy.Host)]
    public ActionResult ResendMail(int mailId)
    {
        var check = mailServer.SendMail(mailId);
        return Json(check);
    }

    [HttpPost]
    [NapAuth(FunctionKey = SiteFunctionKeys.ResendMail, Policy = Constant.Policy.Host)]
    public ActionResult ResendMultipleMails(int[] mailIds)
    {
        foreach (var mailId in mailIds)
        {
            if (mailId > 0)
            {
                mailServer.SendMail(mailId);
            }
        }

        return new EmptyResult();
    }

    [HttpPost]
    [NapAuth(FunctionKey = SiteFunctionKeys.ViewMailDetails, Policy = Constant.Policy.Read)]
    public ActionResult GetMessage(int mailId)
    {
        var mail = mailServer.GetMail(mailId);
        return View(mail);
    }

    [HttpPost]
    public ActionResult GetError(int mailId)
    {
        var mail = mailServer.GetError(mailId);
        return View(mail);
    }

    [HttpPost]
    public ActionResult MailListPagi(
        int status,
        string from,
        string to,
        int page = 0,
        int size = 50,
        int applicationID = 0)
    {
        InitDataMailConfig();

        var totalRows = 0;

        var dateFrom = !string.IsNullOrWhiteSpace(from) ? DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now;
        var dateTo = !string.IsNullOrWhiteSpace(from) ? DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now;

        var listMail = mailServer.WithApp(applicationID)
            .FromDate(dateFrom.ToString(CultureInfo.CurrentCulture))
            .ToDate(dateTo.ToString(CultureInfo.CurrentCulture))
            .WithPagination(page, size)
            .WithStatus(status)
            .GetListMailByDate(ref totalRows);

        var policyViewDetail = userService.GetPermission(napSession.UserId, settings.SiteId, SiteFunctionKeys.ViewMailDetails);
        var policyResend = userService.GetPermission(napSession.UserId, settings.SiteId, SiteFunctionKeys.ResendMail);

        return Json(new
        {
            TotalRows = totalRows,
            HasViewDetail = policyViewDetail,
            HasResend = policyResend,
            ListMail = listMail
        });
    }

    [HttpPost]
    public ActionResult GetMailSetting()
    {
        var listMailSetting = mailServer.GetListSetting();

        return Json(listMailSetting);
    }

    [HttpPost]
    public ActionResult GetMailConfig()
    {
        var listMailConfig = mailServer.GetListConfig();

        return Json(listMailConfig);
    }

    #endregion Ajax

    #region InitData

    private void InitPageSize()
    {
        var listPageSize = new List<SelectListItem>
        {
            new()
            {
                Text = "10",
                Value = "10"
            },
            new()
            {
                Text = "30",
                Value = "30"
            },
            new()
            {
                Text = "50",
                Value = "50"
            },
            new()
            {
                Text = "100",
                Value = "100"
            }
        };

        ViewBag.PageSize = listPageSize;
    }

    private void InitMailStatus()
    {
        var listMailStatus = new List<SelectListItem>
        {
            new()
            {
                Text = MailStatus.All.ToString(),
                Value = ((int)MailStatus.All).ToString()
            },
            new()
            {
                Text = MailStatus.Fail.ToString(),
                Value = ((int)MailStatus.Fail).ToString()
            },
            new()
            {
                Text = MailStatus.Success.ToString(),
                Value = ((int)MailStatus.Success).ToString()
            }
        };

        ViewBag.MailStatus = listMailStatus;
    }

    private void InitDataMailConfig(int appId = 0)
    {
        var listConfig = mailServer.GetListConfig(string.Empty);
        var listConfigPermission = new List<MailConfig>();

        var configPermission = configuration.GetValue<string>("MailPermission:Application");
        var applicationPermissions = configPermission.Split(';');

        foreach (var itemApplicationPermission in applicationPermissions)
        {
            var permission = itemApplicationPermission.Split(':');
            var functionKey = permission[0];
            var applicationId = int.Parse(permission[1]);

            var hasPermission = HasRight(functionKey, Constant.Policy.Read)
                                && napSession.Level == (int)Constant.Level.User;

            if (hasPermission)
            {
                listConfigPermission.AddRange(listConfig.Where(x => x.AppID == applicationId));
            }
        }

        if (listConfigPermission.Any())
        {
            listConfig = listConfigPermission;
        }

        ViewBag.HasLimitedConfigPermission = listConfigPermission.Any();

        ViewBag.ListMailConfig = listConfig.Select(c => new SelectListItem
        {
            Text = c.AppName,
            Value = c.AppID.ToString(),
            Selected = c.AppID == appId
        }).ToList();
    }

    private void InitDataMailSetting()
    {
        var listMailSetting = mailServer.GetListSetting();

        ViewBag.ListMailSetting = listMailSetting.Select(s => new SelectListItem
        {
            Text = s.SettingName,
            Value = s.SettingID.ToString()
        }).ToList();
    }

    private void InitDataIpServer(string serverIp = "")
    {
        var listMailSetting = mailServer.GetListSetting();
        listMailSetting = listMailSetting.GroupBy(s => s.Server).Select(s => s.First()).ToList();

        ViewBag.ListServerIp = listMailSetting.Select(s => new SelectListItem
        {
            Text = s.Server,
            Value = s.Server,
            Selected = s.Server == serverIp
        }).ToList();
    }

    #endregion InitData

    public bool HasRight(string functionKey, Constant.Policy policy)
    {
        var functionPolicy = userService.GetPermission(napSession.UserId, settings.SiteId, functionKey);

        return functionPolicy >= (int)policy;
    }
}