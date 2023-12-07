namespace Maintenance.MailStation.Models;

public class MailTaskStatusCounterModel
{
    public int MissingItemCount { get; set; }

    public int FailedItemCount { get; set; }

    public int RunningItemCount { get; set; }

    public int SucceededItemCount { get; set; }

    public int SentItemCount { get; set; }

    public DateTime CurrentDate { get; set; }
}