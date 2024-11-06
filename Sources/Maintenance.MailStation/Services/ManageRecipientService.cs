namespace Maintenance.MailStation.Services;

using System.Collections.Generic;
using System.Linq;
using Criteria;
using Criteria.MailManage;
using Criteria.MailReport;
using Criteria.ManageRecipient;
using Fanex.Data.Repository;
using Fanex.MailDelivery.Core.Models;
using Fanex.NAP.Client.Models;
using Fanex.NAP.Client.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;

public interface IManageRecipientService
{
    List<RecipientModel> RecipientList(RecipientListCriteria criteria);
    RecipientModel RecipientById(RecipientByIdCriteria criteria);
    CommandResult UpdateRecipient(UpdateRecipientCriteria criteria);
    CommandResult CreateRecipient(CreateRecipientCriteria criteria);
    CommandResult DeleteRecipient(DeleteRecipientCriteria criteria);
    User CheckRecipient(CheckRecipientInfoCriteria criteria);
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

public class ManageRecipientService(IUserService userService , IDynamicRepository dynamicRepository) : MailStationBaseService, IManageRecipientService
{
    public List<RecipientModel> RecipientList(RecipientListCriteria criteria)
    {
        return dynamicRepository.Fetch<RecipientModel>(criteria).ToList();
    }

    public RecipientModel RecipientById(RecipientByIdCriteria criteria)
    {
        return dynamicRepository.Get<RecipientModel>(criteria);
    }

    public CommandResult UpdateRecipient(UpdateRecipientCriteria criteria)
    {
        dynamicRepository.Execute(criteria);

        return new CommandResult(criteria.ErrCode, criteria.ErrMsg);
    }

    public CommandResult CreateRecipient(CreateRecipientCriteria criteria)
    {
        dynamicRepository.Execute(criteria);

        return new CommandResult(criteria.ErrCode, criteria.ErrMsg);
    }

    public CommandResult DeleteRecipient(DeleteRecipientCriteria criteria)
    {
        dynamicRepository.Execute(criteria);

        return new CommandResult(criteria.ErrCode, criteria.ErrMsg);
    }

    public User CheckRecipient(CheckRecipientInfoCriteria criteria)
    {
        return userService.Get(criteria.Username);
    }
}