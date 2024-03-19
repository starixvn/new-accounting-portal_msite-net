namespace Maintenance.BotService.Models.Message
{
    public class MessageLog
    {
        public int MessageId { get; set; }
        public string ConversationBotId { get; set; }
        public string ConversationId { get; set; }
        public string Content { get; set; }
        public DateTime StartSentTime { get; set; }
        public DateTime EndSentTime { get; set; }
        public DateTime InsertedDate { get; set; }
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public double Duration => EndSentTime.Subtract(StartSentTime).TotalMilliseconds;
    }
}