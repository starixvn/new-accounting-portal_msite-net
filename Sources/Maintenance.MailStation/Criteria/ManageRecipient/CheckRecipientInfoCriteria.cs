namespace Maintenance.MailStation.Criteria.ManageRecipient;

using Fanex.Data;
using Fanex.Data.Repository;

public class CheckRecipientInfoCriteria : CriteriaBase
{
    [SpParam(SqlParamName = "RecipientName")]
    public string Username { get; set; }

    public override string GetSettingKey() => "MailStation_MailReport_InsertRecipient";

    public override bool IsValid() => true;
}