using Maintenance.MailStation.Criteria.MailGroup;
using Maintenance.MailStation.Criteria.MailManage;
using Maintenance.MailStation.Criteria.MailReport;
using Maintenance.MailStation.Models;

namespace Maintenance.MailStation.Services;

using Criteria;
using Fanex.Data.Repository;
using Fanex.MailDelivery.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

public interface IMailManageService
{
    IList<MailModel> MailList(GetMailListCriteria criteria);

    MailModel MailById(GetMailByIdCriteria criteria);

    IList<MailActionLogModel> MailLogById(GetMailLogByIdCriteria criteria);

    CommandResult CreateMail(CreateMailCriteria criteria);

    CommandResult UpdateMail(UpdateMailCriteria criteria);

    CommandResult DeleteMail(DeleteMailCriteria criteria);

    CommandResult CreateMailGroup(CreateMailGroupCriteria criteria);

    void RunNow(int mailId);

    List<SelectListItem> Sites(bool hasAllElement = true);

    List<SelectListItem> MailGroups(bool hasAllElement = true);

    List<SelectListItemExtra> Status(bool hasAllElement = true);

    List<SelectListItemExtra> TaskStatus(bool hasAllElement = true);

    List<SelectListItemExtra> Trigger(bool hasAllElement = true);

    List<SuggestMailModel> SuggestMails(SuggestMailCriteria criteria);

    CommandResult ExecuteNonQuery(UpdateCommandBase command);

    MailTaskModel GetMailTaskByID(GetMailTaskByIdCriteria criteria);

    Schedule GetScheduleByMailId(GetScheduleByIdCriteria criteria);

    string GetTimeZone();
}

public class MailManageService(IJobOperationService jobOperationService, IConfiguration configuration, IDynamicRepository dynamicRepository) : MailStationBaseService, IMailManageService
{
    public IList<MailModel> MailList(GetMailListCriteria criteria)
    {
        var clientNotifyTimeOutInSecond = configuration.GetValue<int>("MailStation:ClientNotifyTimeOutInSecond");
        var (mailModels, dates) = dynamicRepository.FetchMultiple<MailModel, CurrentTime>(criteria);
        var now = dates.FirstOrDefault().Now;
        var baseService = new MailStationBaseService();
        var sites = baseService.Sites();
        var mailModelList = mailModels.ToList();
        mailModelList.ForEach(n =>
        {
            var siteName = sites.FirstOrDefault(s => s.Value == n.SiteId.ToString());
            if (siteName != null)
            {
                n.SiteName = siteName.Text;
            }
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

        jobOperationService.RunNow(schedule);
    }
}