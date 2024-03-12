namespace Maintenance.Letstalk.Criteria;

using Fanex.Data.Repository;

public class ResendNotificationCriteria : NonQueryCommand
{
    public int NotificationID { get; set; }

    public int RecipientID { get; set; }

    public int LastModifiedBy { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "Notif_ResendNotification";
}