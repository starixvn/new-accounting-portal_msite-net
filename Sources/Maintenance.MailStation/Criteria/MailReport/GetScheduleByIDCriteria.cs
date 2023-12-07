namespace Maintenance.MailStation.Criteria.MailReport;

using Fanex.Data.Repository;

public class GetScheduleByIdCriteria : CriteriaBase
{
    public int MailId { get; set; }

    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey() => "Mail_MailManagement_GetScheduleByMailId";
}