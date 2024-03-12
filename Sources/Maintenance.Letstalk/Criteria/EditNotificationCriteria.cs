namespace Maintenance.Letstalk.Criteria;

using System;
using Fanex.Data.Repository;

public class EditNotificationCriteria : NonQueryCommand
{
    public int NotificationID { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string LstRecipientID { get; set; }

    public DateTime? ConfigSendTime { get; set; }

    public int LastModifiedBy { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "Notif_EditNotification";
}