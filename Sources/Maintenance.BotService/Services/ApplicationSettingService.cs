namespace Maintenance.BotService.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using Criteria.ApplicationSetting;
using Fanex.Data.Repository;
using Interfaces;
using Models;
using Models.App;

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
        var applicationSetting = await dynamicRepository.GetAsync<ApplicationSetting>(new GetApplicationSettingByIdCriteria { AppId = appSettingId });
        if (applicationSetting is not null)
        {
            applicationSetting.MicrosoftAppPassword = CryptographyHelper.DecryptPassword(applicationSetting.MicrosoftAppPassword);
        }
        return applicationSetting;
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