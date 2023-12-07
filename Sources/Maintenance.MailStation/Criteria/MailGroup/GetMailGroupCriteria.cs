using Fanex.Data.Repository;

namespace Maintenance.MailStation.Criteria.MailGroup;

public class GetMailGroupCriteria : CriteriaBase
{
    public string MailGroupName { get; set; }

    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey() => "MailStation_Group_GetMailGroup";
}