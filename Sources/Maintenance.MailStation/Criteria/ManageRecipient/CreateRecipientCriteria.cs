using Fanex.Data.Repository;

namespace Maintenance.MailStation.Criteria.ManageRecipient;

using Fanex.Data;

public class CreateRecipientCriteria : NonQueryCommand
{
    [SpParam(SqlParamName = "RecipientName")]
    public string UserName { get; set; }

    public string Email1 { get; set; }

    public string Email2 { get; set; }

    public string Email3 { get; set; }

    public bool IsNAPUser { get; set; }

    public int NAPUserID { get; set; }

    public string Note { get; set; }

    [SpParam(SqlParamName = "IsActive")]
    public bool Status { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; } = string.Empty;

    public override string GetSettingKey() => "MailStation_MailReport_InsertRecipient";

    public override bool IsValid() => true;
}