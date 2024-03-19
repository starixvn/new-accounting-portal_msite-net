namespace Maintenance.BotService.Models
{
    public class Bot
    {
        public int AppId { get; set; }
        public int BotId { get; set; }
        public string BotName { get; set; }
        public byte ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string ConversationId { get; set; }
    }
}