namespace Maintenance.Mail.Model;

public class MessageModel
{
    public string Email { get; set; }

    public string Message { get; set; }

    public bool EmailSuccess { get; set; }

    public bool MessageSuccess { get; set; }
}