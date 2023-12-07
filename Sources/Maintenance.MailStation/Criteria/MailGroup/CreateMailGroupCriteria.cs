namespace Maintenance.MailStation.Criteria.MailGroup;

public class CreateMailGroupCriteria : UpdateCommandBase
{
    public string MailGroupName { get; set; }

    public string Description { get; set; }

    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey() => "MailStation_Group_CreateMailGroup";
}