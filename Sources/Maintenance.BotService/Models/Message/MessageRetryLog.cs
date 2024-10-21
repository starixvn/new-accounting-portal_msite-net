namespace Maintenance.BotService.Models.Message;

public class MessageRetryLog
{
    public int MessageId { get; set; }
    public DateTime RetryDate { get; set; }
    public int RetryBy { get; set; }
    public string RetryByUsername { get; set; }
}