namespace Maintenance.BotService.Criteria.Message
{
    public class RetryMessageCommand : BaseCommand
    {
        public int MessageId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }

        public override string GetSettingKey() => "MessageRetryLog_Insert";

        public override bool IsValid() => MessageId > 0 && UserId > 0 && !string.IsNullOrWhiteSpace(Username);
    }
}