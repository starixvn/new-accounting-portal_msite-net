namespace Maintenance.BotService.Controllers
{
    using Fanex.NAP.Client.Attributes;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Business;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Maintenance.BotService.Criteria.ApplicationSetting;
    using Maintenance.BotService.Params;
    using Maintenance.BotService.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    [NapSuperAuth]
    public class ApplicationSettingController(
        IApplicationSettingService applicationSettingService,
        INapSession napSession,
        IUserService userService,
        INapSettings napSettings) : Controller
    {
        [NapAuth(FunctionKey = SiteFunctionKeys.BotAppManagement, Policy = Constant.Policy.Read)]
        public async Task<IActionResult> Index()
        {
            var model = new GetApplicationSettingParams
            {
                Name = string.Empty,
                Active = true,
            };
            ViewBag.HasHostApplicationSettingPermission = await userService.IsHavingPermissionAsync(napSession.UserId, napSettings.SiteId, SiteFunctionKeys.BotAppManagement, Constant.Policy.Host);
            return View(model);
        }

        public async Task<IActionResult> GetApplicationSetting(string name, bool? isActive)
        {
            var apps = await applicationSettingService.GetApplicationSettingAsync(name, isActive);
            return Json(apps);
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.BotAppManagement, Policy = Constant.Policy.Host)]
        public IActionResult Create()
        {
            var viewModel = new InsertApplicationSettingParams();
            return View(viewModel);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.BotAppManagement, Policy = Constant.Policy.Host)]
        public async Task<IActionResult> Create([FromBody] InsertApplicationSettingParams param)
        {
            var command = new InsertApplicationSettingCommand
            {
                MicrosoftAppId = param.MicrosoftAppId,
                AppName = param.ApplicationName,
                MicrosoftAppPassword = param.MicrosoftAppPassword, // Cryptography.EncryptPassword(param.MicrosoftAppPassword),
                Description = param.Description,
                Creator = napSession.UserId,
                IsHideContent = param.HiddenContent
            };

            var result = await applicationSettingService.InsertAsync(command);
            return Json(result);
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.BotAppManagement, Policy = Constant.Policy.Host)]
        public async Task<IActionResult> Update(int appId)
        {
            var appSetting = await applicationSettingService.GetApplicationSettingByIdAsync(appId);
            return View(appSetting);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.BotAppManagement, Policy = Constant.Policy.Host)]
        public async Task<IActionResult> Update([FromBody] UpdateApplicationSettingParams param)
        {
            var command = new UpdateApplicationSettingCommand
            {
                AppId = param.AppId,
                AppName = param.AppName,
                MicrosoftAppPassword = param.MicrosoftAppPassword,
                Description = param.Description,
                IsActive = param.Active,
                IsHideContent = param.HiddenContent,
                UpdatedBy = napSession.UserId
            };
            var result = await applicationSettingService.UpdateAsync(command);
            return Json(result);
        }
    }
}