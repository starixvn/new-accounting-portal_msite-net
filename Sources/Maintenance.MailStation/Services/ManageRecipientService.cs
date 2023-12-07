namespace Maintenance.MailStation.Services;

using System.Collections.Generic;
using System.Linq;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Models;
using Fanex.NAP.Client.Services;
using Maintenance.MailStation.Criteria.ManageRecipient;
using Maintenance.MailStation.Models;

public class ManageRecipientService : MailStationBaseService
{
    private readonly IUserService userService = new UserService();

    private readonly IDynamicRepository dynamicRepository= new DynamicRepository();

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