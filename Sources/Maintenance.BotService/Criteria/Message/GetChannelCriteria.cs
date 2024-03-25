namespace Maintenance.BotService.Criteria.Message
{
    using Fanex.Data.Repository;

    public class GetChannelCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Channel_GetAll";

        public override bool IsValid() => true;
    }
}