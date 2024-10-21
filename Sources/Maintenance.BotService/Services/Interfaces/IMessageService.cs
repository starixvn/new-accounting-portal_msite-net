namespace Maintenance.BotService.Services.Interfaces;

using Criteria.Message;
using Models;
using Models.Message;

public interface IMessageService
{
    Task<IEnumerable<Message>> GetAsync(GetMessageCriteria criteria);

    Task<MessageResend> GetByIdAsync(int messageId);

    Task<IEnumerable<Bot>> GetBotByAppAsync(int appId = 0);

    Task<IEnumerable<Channel>> GetAllChannelAsync();

    Task<IEnumerable<MessageLog>> GetMessageLogAsync(int messageId);

    Task<IEnumerable<MessageRetryLog>> GetMessageRetryLogAsync(int messageId);

    Task<CommandResult> RetryMessageAsync(int messageId, int currentUserId, string currentUsername);
}