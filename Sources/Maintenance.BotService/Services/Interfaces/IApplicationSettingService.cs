namespace Maintenance.BotService.Services.Interfaces;

using Criteria.ApplicationSetting;
using Models;
using Models.App;

public interface IApplicationSettingService
{
    Task<IEnumerable<ApplicationSetting>> GetApplicationSettingAsync(string name, bool? isActive);

    Task<ApplicationSetting> GetApplicationSettingByIdAsync(int appSettingId);

    Task<CommandResult> InsertAsync(InsertApplicationSettingCommand command);

    Task<CommandResult> UpdateAsync(UpdateApplicationSettingCommand command);
}