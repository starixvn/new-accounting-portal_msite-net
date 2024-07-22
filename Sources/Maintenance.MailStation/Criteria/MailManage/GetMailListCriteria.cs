using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.MailStation.Criteria.MailManage;

public class GetMailListCriteria : CriteriaBase
{
    public string MailName { get; set; }

    public int IsActive { get; set; } = -1;

    public string MailGroupId { get; set; }

    public int SiteId { get; set; } = -1;

    public int TriggerType { get; set; } = -1;

    public string Emails { get; set; }

    [SpParam(IsSpecialType = true)]
    public byte[] TransformJson { get; set; }

    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey() => "MailStation_GetMailList";
}