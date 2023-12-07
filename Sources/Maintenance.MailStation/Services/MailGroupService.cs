using Maintenance.MailStation.Criteria.MailGroup;
using Maintenance.MailStation.Models;

namespace Maintenance.MailStation.Services;

using Fanex.Data.Repository;

public class MailGroupService : MailStationBaseService
{
    private readonly IDynamicRepository dynamicRepository = new DynamicRepository();

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