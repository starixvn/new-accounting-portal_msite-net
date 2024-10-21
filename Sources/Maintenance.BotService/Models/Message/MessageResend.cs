namespace Maintenance.BotService.Models.Message;

public class MessageResend
{
    public int MessageId { get; set; }
    public int AppId { get; set; }
    public string ConversationId { get; set; }
    public string Content { get; set; }
    public byte ChannelId { get; set; }
    public string Status { get; set; }
}