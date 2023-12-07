using Fanex.Data.Repository;

namespace Maintenance.MailStation.Criteria.ManageRecipient;

using Fanex.Data;

public class RecipientListCriteria : CriteriaBase
{
    [SpParam(SqlParamName = "RecipientName")]
    public string UserName { get; set; }

    [SpParam(SqlParamName = "Email")]
    public string Emails { get; set; }

    public string Status { get; set; }

    public override string GetSettingKey() => "MailStation_MailReport_GetRecipientList";

    public override bool IsValid() => true;
}