namespace Maintenance.MailStation.Models;

public class RecipientModel
{
    public int RecipientId { get; set; }

    public string RecipientName { get; set; }


    public int EmailId1 { get; set; }

    public int EmailId2 { get; set; }

    public int EmailId3 { get; set; }

    public string Email1 { get; set; }

    public string Email2 { get; set; }

    public string Email3 { get; set; }


    public bool IsNapUser { get; set; }

    public int NapUserId { get; set; }

    public bool IsActive { get; set; }

    public int MailRelevant { get; set; }

    public string Note { get; set; }
}