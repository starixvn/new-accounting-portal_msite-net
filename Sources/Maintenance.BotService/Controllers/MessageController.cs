namespace Maintenance.BotService.Controllers;

using Constants;
using Criteria.Message;
using Fanex.NAP.Client;
using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Params;
using Services.Interfaces;
using static Fanex.NAP.Client.Helpers.Business.Constant;

[SessionAuthorize]
public class MessageController(
    IMessageService messageService,
    IApplicationSettingService applicationSettingService,
    IBotConnector botConnector,
    INapSession napSession,
    IUserService userService,
    INapSettings napSettings) : Controller
{
    [NapAuth(FunctionKey = SiteFunctionKeys.MessageManagement, Policy = Policy.Read)]
    public async Task<IActionResult> Index()
    {
        var model = new GetMessageParams();

        var channels = messageService.GetAllChannelAsync();
        var bots = messageService.GetBotByAppAsync();
        var apps = applicationSettingService.GetApplicationSettingAsync(string.Empty, true);

        await Task.WhenAll(channels, bots, apps);

        ViewBag.ChannelList = await channels;
        ViewBag.BotList = await bots;
        ViewBag.AppList = await apps;
        ViewBag.StatusList = new List<SelectListItem>
        {
            new() { Value = MessageStatus.New, Text = MessageStatus.New},
            new() { Value = MessageStatus.Success, Text = MessageStatus.Success },
            new() { Value = MessageStatus.Failed, Text = MessageStatus.Failed },
            new() { Value = MessageStatus.Resending, Text = MessageStatus.Resending }
        };
        ViewBag.HasHostManageMessage = await userService.IsHavingPermissionAsync(napSession.UserId, napSettings.SiteId, SiteFunctionKeys.MessageManagement, Policy.Host);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetBotByAppId(int appId, byte? channelId)
    {
        var bots = await messageService.GetBotByAppAsync(appId);
        if (channelId is not null)
        {
            bots = bots.Where(b => b.ChannelId == channelId);
        }
        return Json(bots);
    }

    [HttpPost]
    [NapAuth(FunctionKey = SiteFunctionKeys.MessageManagement, Policy = Policy.Read)]
    public async Task<IActionResult> GetMessage([FromBody] GetMessageParams param)
    {
        var criteria = new GetMessageCriteria
        {
            AppId = param.AppId,
            BotId = param.BotId,
            ChannelId = param.ChannelId,
            Status = param.Status,
            PageIndex = param.PageIndex,
            PageSize = param.PageSize,
            FromDate = param.FromDate,
            ToDate = param.ToDate
        };

        var messages = await messageService.GetAsync(criteria);
        return Json(new { TotalItems = criteria.TotalRows, Messages = messages });
    }

    [NapAuth(FunctionKey = SiteFunctionKeys.MessageManagement, Policy = Policy.Read)]
    public async Task<IActionResult> MessageLog(int messageId)
    {
        var logs = await messageService.GetMessageLogAsync(messageId);
        return View(logs);
    }

    [NapAuth(FunctionKey = SiteFunctionKeys.MessageManagement, Policy = Policy.Read)]
    public async Task<IActionResult> MessageRetryLog(int messageId)
    {
        var retryLogs = await messageService.GetMessageRetryLogAsync(messageId);
        return View(retryLogs);
    }

    [HttpPost]
    [NapAuth(FunctionKey = SiteFunctionKeys.MessageManagement, Policy = Policy.Host)]
    public async Task<IActionResult> RetryMessage([FromBody] int messageId)
    {
        var commandResult = await messageService.RetryMessageAsync(messageId, napSession.UserId, napSession.UserName);
        if (commandResult.IsSuccessful)
        {
            try
            {
                var msg = await messageService.GetByIdAsync(messageId);
                await botConnector.RetryMessageAsync(msg);
            }
            catch (Exception ex)
            {
                commandResult.Code = -1;
                commandResult.Message = ex.Message;
            }
        }
        return Json(commandResult);
    }
}