namespace Maintenance.MailStation.Models;

public interface ICommandResult
{
    int Code { get; }

    string Message { get; }

    bool IsSuccessful { get; }
}


public class CommandResult(int code, string message) : ICommandResult
{
    public bool IsSuccessful => Code == 0;

    public CommandResult()
        : this(0, string.Empty)
    {
    }

    public int Code { get; set; } = code;

    public string Message { get; set; } = message;
}