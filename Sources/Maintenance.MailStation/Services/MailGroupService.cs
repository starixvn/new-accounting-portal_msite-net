using Maintenance.MailStation.Criteria.MailGroup;
using Maintenance.MailStation.Models;

namespace Maintenance.MailStation.Services;

using Criteria;
using Criteria.MailManage;
using Criteria.MailReport;
using Fanex.Data.Repository;
using Fanex.MailDelivery.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public interface IMailGroupService
{
    List<MailGroupModel> GetMailGroups(GetMailGroupCriteria criteria);
    CommandResult CreateMailGroup(CreateMailGroupCriteria criteria);
    CommandResult UpdateMailGroup(UpdateMailGroupCriteria criteria);
    CommandResult DeleteMailGroup(DeleteMailGroupCriteria criteria);
    MailGroupModel GetMailGroupById(GetMailGroupByIdCriteria criteria);
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

public class MailGroupService(IDynamicRepository dynamicRepository) : MailStationBaseService, IMailGroupService
{
    public List<MailGroupModel> GetMailGroups(GetMailGroupCriteria criteria)
    {
        return dynamicRepository.Fetch<MailGroupModel>(criteria).ToList();
    }

    public CommandResult CreateMailGroup(CreateMailGroupCriteria criteria)
    {
        return ExecuteNonQuery(criteria);
    }

    public CommandResult UpdateMailGroup(UpdateMailGroupCriteria criteria)
    {
        return ExecuteNonQuery(criteria);
    }

    public CommandResult DeleteMailGroup(DeleteMailGroupCriteria criteria)
    {
        return ExecuteNonQuery(criteria);
    }

    public MailGroupModel GetMailGroupById(GetMailGroupByIdCriteria criteria)
    {
        return dynamicRepository.Get<MailGroupModel>(criteria);
    }
}