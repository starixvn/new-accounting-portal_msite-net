namespace Maintenance.BotService.Services.Interfaces
{
    using Maintenance.BotService.Criteria.ApplicationSetting;
    using Maintenance.BotService.Models;
    using Maintenance.BotService.Models.App;

    public interface IApplicationSettingService
    {
        Task<IEnumerable<ApplicationSetting>> GetApplicationSettingAsync(string name, bool? isDeleted);

        Task<ApplicationSetting> GetApplicationSettingByIdAsync(int appSettingId);

        Task<CommandResult> InsertAsync(InsertApplicationSettingCommand command);

        Task<CommandResult> UpdateAsync(ApplicationSetting applicationSetting);
    }
}