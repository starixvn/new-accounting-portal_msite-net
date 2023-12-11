using Maintenance.MailStation.Criteria.MailGroup;
using Maintenance.MailStation.Criteria.MailManage;
using Maintenance.MailStation.Criteria.MailReport;
using Maintenance.MailStation.Models;

namespace Maintenance.MailStation.Services;

using Fanex.Data.Repository;
using Fanex.MailDelivery.Core.Models;
using Microsoft.Extensions.Configuration;

public class MailManageService(IJobOperationService jobOperationService, IConfiguration configuration, IDynamicRepository dynamicRepository) : MailStationBaseService
{
    public IList<MailModel> MailList(GetMailListCriteria criteria)
    {
        var clientNotifyTimeOutInSecond = configuration.GetValue<int>("MailStation:ClientNotifyTimeOutInSecond");
        var (mailModels, dates) = dynamicRepository.FetchMultiple<MailModel, CurrentTime>(criteria);
        var now = dates.FirstOrDefault().Now;

        var mailModelList = mailModels.ToList();
        mailModelList.ForEach(n =>
        {
            if (n.TriggerType == (int)MailType.Schedule)
            {
                n.IsClientOnline = n.LastActive != null && n.LastActive.Value.AddSeconds(clientNotifyTimeOutInSecond) >= now;
            }
            else
            {
                // Function and others always true
                n.IsClientOnline = true;
            }
        });

        return mailModelList;
    }

    public MailModel MailById(GetMailByIdCriteria criteria)
    {
        return dynamicRepository.Get<MailModel>(criteria);
    }

    public IList<MailActionLogModel> MailLogById(GetMailLogByIdCriteria criteria)
    {
        return dynamicRepository.Fetch<MailActionLogModel>(criteria).ToList();
    }

    public CommandResult CreateMail(CreateMailCriteria criteria)
    {
        return ExecuteNonQuery(criteria);
    }

    public CommandResult UpdateMail(UpdateMailCriteria criteria)
    {
        var result = ExecuteNonQuery(criteria);

        if (result.IsSuccessful && criteria.TriggerType == (int)MailType.Schedule)
        {
            using (jobOperationService = new JobOperationService(configuration))
            {
                jobOperationService.PublishScheduleChange(new Schedule
                {
                    Id = criteria.MailId,
                    Active = criteria.IsActive,
                    EndTime = criteria.EndOn,
                    StartTime = criteria.StartOn ?? DateTime.Now,
                    MaxRetryTimes = criteria.Attempt,
                    Name = criteria.MailName,
                    RepeatExpression = criteria.CronExpression,
                    RetryInterval = criteria.Interval,
                    SiteId = criteria.SiteId
                });
            }
        }

        return result;
    }

    public CommandResult DeleteMail(DeleteMailCriteria criteria)
    {
        return ExecuteNonQuery(criteria);
    }

    public CommandResult CreateMailGroup(CreateMailGroupCriteria criteria)
    {
        return ExecuteNonQuery(criteria);
    }

    public void RunNow(int mailId)
    {
        var schedule = GetScheduleByMailId(new GetScheduleByIdCriteria { MailId = mailId });

        using (jobOperationService = new JobOperationService(configuration))
        {
            jobOperationService.RunNow(schedule);
        }
    }
}