namespace Maintenance.MailStation.Criteria.MailReport;

using Fanex.Data.Repository;

public class GetAllScheduleCriteria : CriteriaBase
{
    public string MailName { get; set; }

    public int SiteId { get; set; }

    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey() => "Mail_Report_GetAllSchedule";
}