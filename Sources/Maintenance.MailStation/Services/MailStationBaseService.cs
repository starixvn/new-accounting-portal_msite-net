using Fanex.Data.Repository;
using Maintenance.MailStation.Constants;
using Maintenance.MailStation.Criteria;
using Maintenance.MailStation.Criteria.MailManage;
using Maintenance.MailStation.Criteria.MailReport;
using Maintenance.MailStation.Models;

namespace Maintenance.MailStation.Services;

using Fanex.MailDelivery.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

public class MailStationBaseService
{
    private readonly IDynamicRepository dynamicRepository = new DynamicRepository();

    public List<SelectListItem> Sites(bool hasAllElement = true)
    {
        var criteria = new GetAllSiteCriteria();
        var result = dynamicRepository.Fetch<SelectListItem>(criteria).ToList();
            
        return hasAllElement ? MailStationConstants.AllItem.Union(result).ToList() : result;
    }

    public List<SelectListItem> MailGroups(bool hasAllElement = true)
    {
        var criteria = new GetAllMailGroupCriteria();
        var result = dynamicRepository.Fetch<SelectListItem>(criteria).ToList();

        return hasAllElement ? MailStationConstants.AllItem.Union(result).ToList() : result;
    }

    public List<SelectListItemExtra> Status(bool hasAllElement = true)
    {
        return hasAllElement ? MailStationConstants.AllItem.Union(MailStationConstants.Active).ToList() : MailStationConstants.Active.ToList();
    }

    public List<SelectListItemExtra> TaskStatus(bool hasAllElement = true)
    {
        return hasAllElement ? MailStationConstants.AllItem.Union(MailStationConstants.TaskStatus).ToList() : MailStationConstants.TaskStatus;
    }

    public List<SelectListItemExtra> Trigger(bool hasAllElement = true)
    {
        return hasAllElement ? MailStationConstants.AllItem.Union(MailStationConstants.TriggerTypes).ToList() : MailStationConstants.TriggerTypes;
    }

    [AllowAnonymous]
    public List<SuggestMailModel> SuggestMails(SuggestMailCriteria criteria)
    {
        return dynamicRepository.Fetch<SuggestMailModel>(criteria).ToList();
    }

    public CommandResult ExecuteNonQuery(UpdateCommandBase command)
    {
        dynamicRepository.Execute(command);
        return new CommandResult(command.ErrCode, command.ErrMsg);
    }

    public MailTaskModel GetMailTaskByID(GetMailTaskByIdCriteria criteria)
    {
        return dynamicRepository.Get<MailTaskModel>(criteria);
    }

    public Schedule GetScheduleByMailId(GetScheduleByIdCriteria criteria)
    {
        return dynamicRepository.Get<Schedule>(criteria);
    }

    public string GetTimeZone()
    {
        return string.Empty;
    }
}