namespace Maintenance.BotService.Params
{
    public class UpdateApplicationSettingParams
    {
        public string AppName { get; set; }
        public string MicrosoftAppPassword { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}