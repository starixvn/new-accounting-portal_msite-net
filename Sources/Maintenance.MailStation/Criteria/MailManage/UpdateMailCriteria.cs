using Maintenance.MailStation.Models;

namespace Maintenance.MailStation.Criteria.MailManage;

public class UpdateMailCriteria : CreateMailCriteria
{
    public UpdateMailCriteria()
    {
    }

    public UpdateMailCriteria(MailModel model)
        : base(model)
    {
        if (model != null)
        {
            MailId = model.MailId;
        }
    }

    public int MailId { get; set; }

    public override string GetSettingKey() => "MailStation_EditMail";
}