namespace Maintenance.Letstalk.Criteria;

using System;
using Fanex.Data.Repository;

public class InsertNotificationCriteria : NonQueryCommand
{
    public string Title { get; set; }

    public string Content { get; set; }

    public string LstRecipientID { get; set; }

    public DateTime? ConfigSendTime { get; set; }

    public int CreatedBy { get; set; }

    public bool IsTest { get; set; }
        
    public override bool IsValid() => true;

    public override string GetSettingKey() => "Notif_InsertNewNotification";
}