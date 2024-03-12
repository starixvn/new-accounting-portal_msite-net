namespace Maintenance.Letstalk.Criteria;

using System;
using Fanex.Data.Repository;

public class GetNotificationListCriteria : CriteriaBase
{
    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public int? RecipientID { get; set; }

    public byte? StatusId { get; set; }

    public int? NotificationId { get; set; }

    public bool IsTest { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "Notif_GetListNotification";
}