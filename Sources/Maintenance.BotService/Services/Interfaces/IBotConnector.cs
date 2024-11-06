namespace Maintenance.BotService.Services.Interfaces;

using Models.Message;

public interface IBotConnector
{
    Task RetryMessageAsync(MessageResend message);
}