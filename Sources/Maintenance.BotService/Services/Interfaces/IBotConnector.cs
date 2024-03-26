namespace Maintenance.BotService.Services.Interfaces
{
    using Maintenance.BotService.Models.Message;

    public interface IBotConnector
    {
        Task RetryMessageAsync(MessageResend message);
    }
}