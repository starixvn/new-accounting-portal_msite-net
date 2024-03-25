namespace Maintenance.BotService.Services.Interfaces
{
    using Maintenance.BotService.Criteria.Message;
    using Maintenance.BotService.Models;
    using Maintenance.BotService.Models.Message;

    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetAsync(GetMessageCriteria criteria);

        Task<IEnumerable<Bot>> GetBotByAppAsync(int appId = 0);

        Task<IEnumerable<Channel>> GetAllChannelAsync();

        Task<IEnumerable<MessageLog>> GetMessageLogAsync(int messageId);
    }
}