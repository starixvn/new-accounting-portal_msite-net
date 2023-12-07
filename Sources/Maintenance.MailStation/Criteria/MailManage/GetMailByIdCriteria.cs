using Fanex.Data.Repository;

namespace Maintenance.MailStation.Criteria.MailManage;

public class GetMailByIdCriteria : CriteriaBase
{
    public int MailId { get; set; }

    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey() => "MailStation_GetMailByID";
}