namespace Maintenance.BotService.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Maintenance.BotService.Criteria.Message;
    using Maintenance.BotService.Models;
    using Maintenance.BotService.Models.Message;
    using Maintenance.BotService.Services.Interfaces;

    public class MessageService(IDynamicRepository dynamicRepository) : IMessageService
    {
        public async Task<IEnumerable<Channel>> GetAllChannelAsync()
            => await dynamicRepository.FetchAsync<Channel>(new GetChannelCriteria());

        public async Task<IEnumerable<Bot>> GetBotByAppAsync(int appId = 0)
            => await dynamicRepository.FetchAsync<Bot>(new GetBotByAppIdCriteria { AppId = appId });

        public async Task<IEnumerable<Message>> GetAsync(GetMessageCriteria criteria)
            => await dynamicRepository.FetchAsync<Message>(criteria);

        public async Task<IEnumerable<MessageLog>> GetMessageLogAsync(int messageId)
            => await dynamicRepository.FetchAsync<MessageLog>(new GetLogMessageCriteria { MessageId = messageId });
    }
}