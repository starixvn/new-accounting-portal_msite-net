using Fanex.Data.Repository;

namespace Maintenance.MailStation.Criteria.MailManage;

public class GetAllSiteCriteria : CriteriaBase
{
    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey() => "MailStation_GetAllSites";
}