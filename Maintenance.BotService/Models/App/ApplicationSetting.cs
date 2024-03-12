namespace Maintenance.BotService.Models.App
{
    public class ApplicationSetting
    {
        public int AppId { get; set; }
        public string AppName { get; set; }
        public string MicrosoftAppId { get; set; }
        public string MicrosoftAppPassword { get; set; }
        public string MicrosoftAppType { get; set; }
        public string MicrosoftAppTenantId { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
    }
}