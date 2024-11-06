namespace Maintenance.Mail.Services;

using System.Collections.Generic;
using Criteria.MailService;
using Fanex.Data.Repository;

public interface IMailRepository
{
    IEnumerable<MailConfigLogModel> GetMailConfigLogs(int id);
}

public class MailRepository(IDynamicRepository dynamicRepository) : IMailRepository
{
    public IEnumerable<MailConfigLogModel> GetMailConfigLogs(int id)
    {
        var criteria = new GetMailConfigLogsCriteria { AppID = id };
        return dynamicRepository.Fetch<MailConfigLogModel>(criteria).ToList();
    }
}