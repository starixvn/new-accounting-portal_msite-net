namespace Maintenance.MailStation.Criteria.MailReport;

using Fanex.Data.Repository;

public class MailDetailCriteria : CriteriaBase
{
    public int MailId { get; set; }

    public override string GetSettingKey() => "MailStation_MailReport_GetMailDetail";

    public override bool IsValid() => true;
}