namespace Maintenance.MailStation.Models;

public class MailModel
{
    public int MailId { get; set; }

    public string MailName { get; set; }

    public int MailGroupId { get; set; }

    public string MailGroupName { get; set; }

    public int SiteId { get; set; }

    public string SiteName { get; set; }

    public int TriggerType { get; set; }

    public string TriggerTypeName
    {
        get
        {
            return TriggerType switch
            {
                0 => "Function",
                1 => "Schedule",
                _ => "Unknown"
            };
        }
    }

    public int RepeatType { get; set; }

    public string CronExpression { get; set; }

    public string MailFrom { get; set; }

    public string MailTo { get; set; }

    public string MailCc { get; set; }

    public string MailBcc { get; set; }

    public bool IsActive { get; set; }

    public DateTime? LastSent { get; set; }

    public DateTime? StartOn { get; set; }

    public DateTime? EndOn { get; set; }

    public int Attempt { get; set; }

    public int Interval { get; set; }

    public string Description { get; set; }

    public string Owners { get; set; }

    public DateTime? LastActive { get; set; }

    public bool IsClientOnline { get; set; }
}