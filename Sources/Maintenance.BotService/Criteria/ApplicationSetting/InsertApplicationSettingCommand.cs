namespace Maintenance.BotService.Criteria.ApplicationSetting;

public class InsertApplicationSettingCommand : BaseCommand
{
    public string AppName { get; set; }
    public string MicrosoftAppId { get; set; }
    public string MicrosoftAppPassword { get; set; }
    public string MicrosoftAppType { get; set; } = "MultiTenant";
    public string MicrosoftAppTenantId { get; set; } = string.Empty;
    public string Description { get; set; }
    public bool IsHideContent { get; set; }
    public int Creator { get; set; }

    public override string GetSettingKey() => "ApplicationSetting_Insert";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(AppName) &&
                                      !string.IsNullOrWhiteSpace(MicrosoftAppId) &&
                                      !string.IsNullOrWhiteSpace(MicrosoftAppPassword) &&
                                      Creator > 0;
}