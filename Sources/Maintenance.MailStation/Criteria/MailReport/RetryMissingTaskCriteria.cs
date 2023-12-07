namespace Maintenance.MailStation.Criteria.MailReport;

using System;
using Fanex.Data.Repository;

public class RetryMissingTaskCriteria : NonQueryCommand
{
    public int MailTaskId { get; set; }

    public int MailId { get; set; }

    public int SiteId { get; set; }

    public DateTime ScheduledTime { get; set; }

    public DateTime? LastSent { get; set; }

    public int Status { get; set; }

    public int Attempt { get; set; }

    public int MSiteMailId { get; set; }

    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey() => "Mail_Core_InsertMailTask";
}