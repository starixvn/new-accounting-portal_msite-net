namespace Maintenance.BotService.Criteria.Message
{
    using Fanex.Data.Repository;

    public class GetLogMessageCriteria : CriteriaBase
    {
        public int MessageId { get; set; }

        public override string GetSettingKey() => "MessageLog_Get";

        public override bool IsValid() => MessageId > 0;
    }
}