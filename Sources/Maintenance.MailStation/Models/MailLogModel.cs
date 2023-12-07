namespace Maintenance.MailStation.Models;

public class MailLogModel
{
    public int LogId { get; set; }

    public int TaskId { get; set; }

    public int Status { get; set; }

    public string ExceptionMessage { get; set; }

    public string LogTime { get; set; }

    public string StackTrace { get; set; }
}