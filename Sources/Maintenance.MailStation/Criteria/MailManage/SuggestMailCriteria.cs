namespace Maintenance.MailStation.Criteria.MailManage;

using Fanex.Data;
using Fanex.Data.Repository;

public class SuggestMailCriteria : CriteriaBase
{
    [SpParam(SqlParamName = "EmailAddress")]
    public string Query { get; set; }

    public override string GetSettingKey() => "MailStation_GetAllSuggestedMails";

    public override bool IsValid() => true;
}