using Fanex.Data.Repository;

namespace Maintenance.MailStation.Criteria.MailGroup;

public class GetMailGroupByIdCriteria : CriteriaBase
{
    public int MailGroupId { get; set; }

    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey() => "MailStation_Group_GetMailGroupByID";
}