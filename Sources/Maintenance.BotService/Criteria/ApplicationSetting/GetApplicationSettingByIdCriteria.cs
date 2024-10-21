namespace Maintenance.BotService.Criteria.ApplicationSetting;

using Fanex.Data.Repository;

public class GetApplicationSettingByIdCriteria : CriteriaBase
{
    public int AppId { get; set; }

    public override string GetSettingKey() => "ApplicationSetting_GetById";

    public override bool IsValid() => AppId > 0;
}