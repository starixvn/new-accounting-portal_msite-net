using Fanex.Data.Repository;

namespace Maintenance.MailStation.Criteria.ManageRecipient;

using Fanex.Data;

public class RecipientByIdCriteria : CriteriaBase
{
    [SpParam(SqlParamName = "RecipientID")]
    public int RecipientId { get; set; }

    public override string GetSettingKey() => "MailStation_MailReport_GetRecipientById";

    public override bool IsValid() => true;
}