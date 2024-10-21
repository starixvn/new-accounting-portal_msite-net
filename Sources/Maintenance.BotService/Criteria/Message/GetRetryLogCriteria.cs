namespace Maintenance.BotService.Criteria.Message;

using Fanex.Data.Repository;

public class GetRetryLogCriteria : CriteriaBase
{
    public int MessageId { get; set; }

    public override string GetSettingKey() => "MessageRetryLog_Get";

    public override bool IsValid() => MessageId > 0;
}