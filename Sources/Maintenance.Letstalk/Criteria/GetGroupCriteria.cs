namespace Maintenance.Letstalk.Criteria;

using Fanex.Data.Repository;

public class GetGroupCriteria : CriteriaBase
{
    public override bool IsValid() => true;

    public override string GetSettingKey() => "Notif_GetListRecipients";
}