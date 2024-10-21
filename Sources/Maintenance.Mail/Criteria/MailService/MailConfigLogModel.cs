namespace Maintenance.Mail.Criteria.MailService;

using System;

public class MailConfigLogModel
{
    public int AppID { get; set; }

    public string AppName { get; set; }

    public string Description { get; set; }

    public string SettingName { get; set; }

    public string MailFrom { get; set; }

    public bool IsEncrypt { get; set; }

    public DateTime LogTime { get; set; }

    public string Username { get; set; }
}