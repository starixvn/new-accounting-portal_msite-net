namespace Maintenance.BotService.Controllers
{
    using Fanex.NAP.Client.Attributes;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Maintenance.BotService.Constants;
    using Maintenance.BotService.Criteria.Message;
    using Maintenance.BotService.Params;
    using Maintenance.BotService.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using static Fanex.NAP.Client.Helpers.Business.Constant;

    [NapSuperAuth]
    public class MessageController(
        IMessageService messageService,
        IApplicationSettingService applicationSettingService,
        INapSession napSession,
        IUserService userService,
        INapSettings napSettings) : Controller
    {
        public async Task<IActionResult> Index()
        {
            DateTime current = DateTime.Now;
            var model = new GetMessageParams
            {
                FromDate = current.AddDays(-7),
                ToDate = current
            };

            var channels = messageService.GetAllChannelAsync();
            var bots = messageService.GetBotByAppAsync();
            var apps = applicationSettingService.GetApplicationSettingAsync(string.Empty, true);

            await Task.WhenAll(channels, bots, apps);

            ViewBag.ChannelList = await channels;
            ViewBag.BotList = await bots;
            ViewBag.AppList = await apps;
            ViewBag.StatusList = new List<SelectListItem>()
            {
                new() {Value = MessageStatus.Failed, Text = MessageStatus.Failed},
                new() {Value = MessageStatus.Success, Text = MessageStatus.Success}
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
                ToDate = param.ToDate,
            };

            var messages = await messageService.GetAsync(criteria);
            return Json(new { TotalItems = criteria.TotalRows, Messages = messages });
        }

        public async Task<IActionResult> MessageLog(int messageId)
        {
            var logs = await messageService.GetMessageLogAsync(messageId);
            return View(logs);
        }
    }
}