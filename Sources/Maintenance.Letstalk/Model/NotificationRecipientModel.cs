namespace Maintenance.Letstalk.Model;

using System;

public class NotificationRecipientModel
{
    public int NotificationID { get; set; }

    public int RecipientID { get; set; }

    public string ConversationID { get; set; }

    public string RecipientName { get; set; }

    public string Status { get; set; }

    public DateTime? LastSentTime { get; set; }
}