using Fanex.Data.Repository;

namespace Maintenance.MailStation.Criteria.ManageRecipient;

using Fanex.Data;

public class UpdateRecipientCriteria : NonQueryCommand
{
    [SpParam(SqlParamName = "RecipientID")]
    public int RecipientId { get; set; }

    [SpParam(SqlParamName = "RecipientName")]
    public string Username { get; set; }

    public int EmailID1 { get; set; }

    public int EmailID2 { get; set; }

    public int EmailID3 { get; set; }

    public string Email1 { get; set; }

    public string Email2 { get; set; }

    public string Email3 { get; set; }

    public string Note { get; set; }

    [SpParam(SqlParamName = "IsActive")]
    public bool Status { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; } = string.Empty;

    public override string GetSettingKey() => "MailStation_MailReport_UpdateRecipient";

    public override bool IsValid() => true;
}