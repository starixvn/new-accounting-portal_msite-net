namespace Maintenance.BotService.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Maintenance.BotService.Criteria.ApplicationSetting;
    using Maintenance.BotService.Models;
    using Maintenance.BotService.Models.App;
    using Maintenance.BotService.Services.Interfaces;

    public class ApplicationSettingService(IDynamicRepository dynamicRepository) : IApplicationSettingService
    {
        public async Task<IEnumerable<ApplicationSetting>> GetApplicationSettingAsync(string name, bool? isActive)
        {
            var (apps, bots) = await dynamicRepository.FetchMultipleAsync<ApplicationSetting, Bot>(new GetApplicationSettingCriteria { Name = name, IsActive = isActive });
            foreach (var app in apps)
            {
                app.Bots = bots.Where(b => b.AppId == app.AppId);
            }
            return apps;
        }

        public async Task<ApplicationSetting> GetApplicationSettingByIdAsync(int appSettingId)
        {
            var appSetting = await dynamicRepository.GetAsync<ApplicationSetting>(new GetApplicationSettingByIdCriteria { AppId = appSettingId });
            if (appSetting is not null)
            {
                appSetting.MicrosoftAppPassword = Cryptography.EncryptPassword(appSetting.MicrosoftAppPassword);
            }
            return appSetting;
        }

        public async Task<CommandResult> InsertAsync(InsertApplicationSettingCommand command)
        {
            await dynamicRepository.ExecuteAsync(command);
            var result = new CommandResult
            {
                Code = command.ErrCode,
                Message = command.ErrMsg
            };
            return result;
        }

        public async Task<CommandResult> UpdateAsync(UpdateApplicationSettingCommand command)
        {
            await dynamicRepository.ExecuteAsync(command);
            var result = new CommandResult
            {
                Code = command.ErrCode,
                Message = command.ErrMsg
            };
            return result;
        }
    }
}