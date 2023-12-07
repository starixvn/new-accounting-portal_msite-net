namespace Maintenance.MailStation.Criteria.MailReport;

using Fanex.Data.Repository;

public class GetMailTaskByIdCriteria : CriteriaBase
{
    public int MailTaskId { get; set; }

    public override string GetSettingKey() => "Mail_Report_GetMailTaskByID";

    public override bool IsValid() => true;
}