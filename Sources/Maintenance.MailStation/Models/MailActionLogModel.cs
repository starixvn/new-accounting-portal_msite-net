namespace Maintenance.MailStation.Models;

public class MailActionLogModel
{
    public int MailLogId { get; set; }

    public string Message { get; set; }

    public DateTime LogTime { get; set; }

    public int MailId { get; set; }
}