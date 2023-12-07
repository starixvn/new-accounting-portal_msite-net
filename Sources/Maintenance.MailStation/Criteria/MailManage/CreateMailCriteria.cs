using Maintenance.MailStation.Models;

namespace Maintenance.MailStation.Criteria.MailManage;

public class CreateMailCriteria() : UpdateCommandBase
{
    public CreateMailCriteria(MailModel model)
        : this()
    {
        if (model != null)
        {
            MailName = model.MailName;
            MailGroupId = model.MailGroupId;
            SiteId = model.SiteId;
            TriggerType = model.TriggerType;
            RepeatType = model.RepeatType;
            CronExpression = model.CronExpression;
            MailFrom = model.MailFrom;
            MailTo = model.MailTo;
            MailCc = model.MailCc;
            MailBcc = model.MailBcc;
            IsActive = model.IsActive;
            StartOn = model.StartOn;
            EndOn = model.EndOn;
            Attempt = model.Attempt;
            Interval = model.Interval;
            Description = model.Description;
            Owners = model.Owners;
        }
        else
        {
            TriggerType = 1;
        }
    }

    public string MailName { get; set; }

    public int MailGroupId { get; set; }

    public int SiteId { get; set; }

    public int TriggerType { get; set; }

    public int RepeatType { get; set; }

    public string CronExpression { get; set; }

    public string MailFrom { get; set; }

    public string MailTo { get; set; }

    public string MailCc { get; set; }

    public string MailBcc { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? StartOn { get; set; }

    public DateTime? EndOn { get; set; }

    public int Attempt { get; set; }

    public int Interval { get; set; }

    public string Description { get; set; }

    public string Owners { get; set; }

    public string ModifiedBy { get; set; }

    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey() => "MailStation_CreateMail";
}