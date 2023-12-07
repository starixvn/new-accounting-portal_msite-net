namespace Maintenance.MailStation.Criteria.MailReport;

public class AddMailLogCriteria : UpdateCommandBase
{
    public int TaskId { get; set; }

    public string ExceptionMessage { get; set; }

    public string StackTrace { get; set; }

    public int Status { get; set; }

    public DateTime LogTime { get; set; }

    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey() => "Mail_Core_InsertMailLogs";
}