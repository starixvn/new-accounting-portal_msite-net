using Maintenance.MailStation.Criteria.MailReport;
using Maintenance.MailStation.Models;

namespace Maintenance.MailStation.Services;

using Fanex.Data.Repository;
using Fanex.MailDeliver.Model;
using Fanex.MailDelivery.Core.Models;
using Maintenance.Services;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Spi;

public class MailReportService(IDynamicRepository dynamicRepository, IConfiguration configuration, IMailService mailService) : MailStationBaseService
{
    private IJobOperationService jobOperationService;

    public List<MailTaskModel> MailReport(MailReportCriteria criteria)
    {
        var oldStatus = criteria.Status;
        criteria.Status = -1;
        var mailTasks = GetMailTasks(criteria);

        if (criteria.TriggerType != (int)MailType.Function)
        {
            criteria.Status = oldStatus;
            var getAllScheduleCriteria = new GetAllScheduleCriteria { MailName = criteria.MailName, SiteId = criteria.SiteId };
            var mailTaskDetails = GetMailTaskDetails(getAllScheduleCriteria);

            if (criteria.Status is -1 or (int)JobStatus.Waiting or (int)JobStatus.Missing)
            {
                var ts = new TimeSpan(23, 59, 59);
                criteria.ToDate += ts;

                foreach (var schedule in mailTaskDetails)
                {
                    var fromDate = MaxDateTime(criteria.FromDate, schedule.StartOn).ToUniversalTime();
                    var toDate = MinDateTime(criteria.ToDate, schedule.EndOn).ToUniversalTime();

                    if (fromDate <= toDate)
                    {
                        var trigger = TriggerBuilder.Create().WithCronSchedule(schedule.CronExpression).Build();
                        var triggerTimes = TriggerUtils.ComputeFireTimesBetween(
                            (IOperableTrigger)trigger,
                            null,
                            new DateTimeOffset(fromDate),
                            new DateTimeOffset(toDate));
                        triggerTimes = (from time in triggerTimes
                            where !(from task in mailTasks
                                    where task.MailId == schedule.MailId
                                    select task.ScheduledTime)
                                .Contains(time.LocalDateTime)
                            select time).ToList();

                        foreach (var time in triggerTimes)
                        {
                            var status = time.LocalDateTime.AddSeconds(5) > DateTime.Now ? (int)JobStatus.Waiting : (int)JobStatus.Missing;

                            if (criteria.Status == -1 || criteria.Status == status)
                            {
                                var waitingTask = new MailTaskModel
                                {
                                    MailName = schedule.MailName,
                                    MailId = schedule.MailId,
                                    Attempt = schedule.Attempt,
                                    LastSent = null,
                                    ScheduledTime = time.LocalDateTime,
                                    SiteId = schedule.SiteId,
                                    SiteName = schedule.SiteName,
                                    Status = status,
                                    TriggerType = (int)MailType.Schedule
                                };

                                mailTasks.Add(waitingTask);
                            }
                        }
                    }
                }
            }
        }

        if (oldStatus != -1)
        {
            mailTasks = mailTasks.Where(m => m.Status == oldStatus).ToList();
        }

        return mailTasks.OrderBy(m => m.MailName).ThenByDescending(m => m.ScheduledTime).ThenByDescending(m => m.LastSent).ToList();
    }

    public MailTaskStatusCounterModel GetMailTaskStatusCounter()
    {
        var criteria = new MailReportCriteria
        {
            FromDate = DateTime.Now.Date,
            ToDate = DateTime.Now.Date,
            MailName = null,
            SiteId = -1,
            Status = -1,
            TriggerType = -1
        };

        var mailTasks = MailReport(criteria);

        var counter = new MailTaskStatusCounterModel
        {
            MissingItemCount = mailTasks.Count(x => x.Status == (int)JobStatus.Missing),
            FailedItemCount = mailTasks.Count(x => x.Status == (int)JobStatus.JobFailed),
            RunningItemCount = mailTasks.Count(x => x.Status == (int)JobStatus.Running),
            SentItemCount = mailTasks.Count(x => x.Status == (int)JobStatus.Success),
            SucceededItemCount = mailTasks.Count(x => x.Status == (int)JobStatus.SuccessWithoutSendmail),
            CurrentDate = DateTime.Now.Date
        };

        return counter;
    }

    private List<MailTaskDetailModel> GetMailTaskDetails(GetAllScheduleCriteria getAllScheduleCriteria)
    {
        return dynamicRepository.Fetch<MailTaskDetailModel>(getAllScheduleCriteria).ToList();
    }

    private List<MailTaskModel> GetMailTasks(MailReportCriteria criteria)
    {
        return dynamicRepository.Fetch<MailTaskModel>(criteria).ToList();
    }

    public MailRecord MailDetail(MailDetailCriteria criteria)
    {
        return mailService.GetMail(criteria.MailId);
    }

    public List<MailLogModel> MailLog(MailLogCriteria criteria)
    {
        return dynamicRepository.Fetch<MailLogModel>(criteria).ToList();
    }

    public void RetrySendMail(RetrySendMailCriteria criteria)
    {
        var mailTask = GetMailTaskByID(new GetMailTaskByIdCriteria { MailTaskId = criteria.TaskId });
        var schedule = GetScheduleByMailId(new GetScheduleByIdCriteria { MailId = mailTask.MailId });
        schedule.TaskId = mailTask.MailTaskId;

        UpdateRunningTask(mailTask.MailTaskId);

        using (jobOperationService = new JobOperationService(configuration))
        {
            jobOperationService.Retry(schedule);
        }
    }

    public void AbortTask(int taskId)
    {
        var criteria = new UpdateMailTaskStatusCriteria
        {
            TaskId = taskId,
            Status = (int)JobStatus.Abort,
            LastSent = DateTime.Now
        };

        ExecuteNonQuery(criteria);
    }

    public void AddMailLog(AddMailLogCriteria criteria)
    {
        ExecuteNonQuery(criteria);
    }

    public void RetryMissingJob(RetryMissingTaskCriteria criteria)
    {
        dynamicRepository.Execute(criteria);
        var mailTaskId = criteria.MailTaskId;

        var mailTask = GetMailTaskByID(new GetMailTaskByIdCriteria { MailTaskId = mailTaskId });
        var schedule = GetScheduleByMailId(new GetScheduleByIdCriteria { MailId = mailTask.MailId });
        schedule.CurrentFireTime = criteria.ScheduledTime;
        schedule.TaskId = mailTaskId;

        using (jobOperationService = new JobOperationService(configuration))
        {
            jobOperationService.Retry(schedule);
        }
    }

    private void UpdateRunningTask(int taskId)
    {
        var criteria = new UpdateMailTaskStatusCriteria
        {
            TaskId = taskId,
            Status = (int)JobStatus.Running,
            LastSent = DateTime.Now
        };

        ExecuteNonQuery(criteria);
    }

    private static DateTime MaxDateTime(DateTime date1, DateTime? date2)
    {
        return date2.HasValue
            ?
            // Add 5 seconds for the number ratio.
            date1 >= date2.Value ? date1 : date2.Value.AddSeconds(5)
            : date1;
    }

    private static DateTime MinDateTime(DateTime date1, DateTime? date2)
    {
        return date2.HasValue
            ?
            // Add 5 seconds for the number ratio.
            date1 < date2.Value ? date1 : date2.Value.AddSeconds(5)
            : date1;
    }
}