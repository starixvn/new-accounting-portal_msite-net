using Fanex.Data.Repository;

namespace Maintenance.MailStation.Criteria.ManageRecipient;

using Fanex.Data;

public class DeleteRecipientCriteria : NonQueryCommand
{
    [SpParam(SqlParamName = "RecipientID")]
    public int RecipientId { get; set; }

    public int ErrCode { get; set; } = 0;

    public string ErrMsg { get; set; } = string.Empty;

    public override string GetSettingKey() => "MailStation_MailReport_DeleteRecipient";

    public override bool IsValid() => true;
}