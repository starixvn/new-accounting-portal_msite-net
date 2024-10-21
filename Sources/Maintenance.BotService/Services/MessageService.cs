namespace Maintenance.BotService.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using Criteria.Message;
using Fanex.Data.Repository;
using Interfaces;
using Models;
using Models.Message;

public class MessageService(IDynamicRepository dynamicRepository) : IMessageService
{
    public async Task<IEnumerable<Channel>> GetAllChannelAsync()
        => await dynamicRepository.FetchAsync<Channel>(new GetChannelCriteria());

    public async Task<IEnumerable<Bot>> GetBotByAppAsync(int appId = 0)
        => await dynamicRepository.FetchAsync<Bot>(new GetBotByAppIdCriteria { AppId = appId });

    public async Task<IEnumerable<Message>> GetAsync(GetMessageCriteria criteria)
    {
        var (messages, attachments) = await dynamicRepository.FetchMultipleAsync<Message, Attachment>(criteria);
        foreach (var message in messages)
        {
            var listAtt = attachments.Where(a => a.MessageId == message.MessageId);
            message.Attachments = listAtt;
        }
        return messages;
    }

    public async Task<IEnumerable<MessageLog>> GetMessageLogAsync(int messageId)
        => await dynamicRepository.FetchAsync<MessageLog>(new GetLogMessageCriteria { MessageId = messageId });

    public async Task<CommandResult> RetryMessageAsync(int messageId, int currentUserId, string currentUsername)
    {
        RetryMessageCommand command = new()
        {
            MessageId = messageId,
            UserId = currentUserId,
            Username = currentUsername
        };
        await dynamicRepository.ExecuteAsync(command);

        var result = new CommandResult
        {
            Code = command.ErrCode,
            Message = command.ErrMsg
        };
        return result;
    }

    public async Task<MessageResend> GetByIdAsync(int messageId)
        => await dynamicRepository.GetAsync<MessageResend>(new GetMessageByIdCriteria { MessageId = messageId });

    public async Task<IEnumerable<MessageRetryLog>> GetMessageRetryLogAsync(int messageId)
        => await dynamicRepository.FetchAsync<MessageRetryLog>(new GetRetryLogCriteria { MessageId = messageId });
}