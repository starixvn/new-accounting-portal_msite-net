namespace Maintenance.BotService.Criteria.Message
{
    using Fanex.Data.Repository;

    public class GetBotByAppIdCriteria : CriteriaBase
    {
        public int AppId { get; set; }

        public override string GetSettingKey() => "Bot_GetByAppId";

        public override bool IsValid() => AppId >= 0;
    }
}