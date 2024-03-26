namespace Maintenance.BotService.Models.Message
{
    public class Message
    {
        public int MessageId { get; set; }
        public byte ChannelId { get; set; }
        public string ChannelName { get; set; }
        public int AppId { get; set; }
        public string AppName { get; set; }
        public int BotId { get; set; }
        public string BotName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public bool IsHideContent { get; set; }
        public int Retry { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime SentDate { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}