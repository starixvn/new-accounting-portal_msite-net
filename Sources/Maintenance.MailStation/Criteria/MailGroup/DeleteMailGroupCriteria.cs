namespace Maintenance.MailStation.Criteria.MailGroup;

public class DeleteMailGroupCriteria : UpdateCommandBase
{
    public int MailGroupId { get; set; }

    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey() => "MailStation_Group_DeleteMailGroup";
}