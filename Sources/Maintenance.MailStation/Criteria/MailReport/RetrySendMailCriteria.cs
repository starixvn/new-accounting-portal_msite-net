namespace Maintenance.MailStation.Criteria.MailReport;

using Fanex.Data.Repository;

public class RetrySendMailCriteria : CriteriaBase
{
    public int TaskId { get; set; }

    public override string GetSettingKey() => "MailStation_MailReport_GetMailTaskList";

    public override bool IsValid() => true;
}