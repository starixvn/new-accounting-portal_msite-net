namespace Maintenance.BotService.Criteria.ApplicationSetting
{
    using Fanex.Data.Repository;

    public class GetApplicationSettingCriteria : CriteriaBase
    {
        public string Name { get; set; }
        public bool? IsDeleted { get; set; }

        public override string GetSettingKey() => "ApplicationSetting_GetAll";

        public override bool IsValid() => true;
    }
}