namespace Maintenance.Letstalk.Model;

using System;

public class NotificationModel
{
    private const string TimeFormat = "MMM dd, yyyy HH:mm:ss";

    public int NotificationID { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public int RecipientID { get; set; }

    public string ConversationID { get; set; }

    public string RecipientName { get; set; }

    public int NumberAttachment { get; set; }

    public byte NotificationType { get; set; }

    public DateTime ConfigSendTime { get; set; }

    public string ConfigSendTimeString => ConfigSendTime.ToString(TimeFormat);

    public DateTime? LastSentTime { get; set; }

    public string LastSentTimeString => !LastSentTime.HasValue ? "" : LastSentTime.Value.ToString(TimeFormat);

    public string Status { get; set; }

    public DateTime LastUpdTime { get; set; }

    public string LastUpdTimeString => LastUpdTime.ToString(TimeFormat);

    public DateTime CreatedTime { get; set; }

    public string CreatedTimeString => CreatedTime.ToString(TimeFormat);

    public int CreatedBy { get; set; }

    public string Creator { get; set; }

    public int LastModifiedBy { get; set; }

    public string LatestEditor { get; set; }

    public byte IsEdit { get; set; }

    public byte IsDelete { get; set; }
}