namespace Maintenance.Letstalk.Model;

public class RecipientModel
{
    public int RecipientID { get; set; }

    public string RecipientName { get; set; }

    public string ConversationID { get; set; }

    public string Channel { get; set; }

    public string Environment { get; set; }

    public string Remark { get; set; }

    public bool IsDelete { get; set; }

    public string Tag { get; set; }

    public string Target { get; set; }
}