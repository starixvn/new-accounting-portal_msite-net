namespace Maintenance.MailStation.Criteria.MailManage;

public class DeleteMailCriteria : UpdateCommandBase
{
    public int MailId { get; set; }

    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey() => "MailStation_DeleteMail";
}