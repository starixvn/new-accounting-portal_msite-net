namespace Maintenance.BotService.Criteria.ApplicationSetting
{
    public class UpdateApplicationSettingCommand : BaseCommand
    {
        public int AppId { get; set; }
        public string AppName { get; set; }
        public string MicrosoftAppPassword { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsHideContent { get; set; }
        public int UpdatedBy { get; set; }

        public override string GetSettingKey() => "ApplicationSetting_Update";

        public override bool IsValid() => true;
    }
}