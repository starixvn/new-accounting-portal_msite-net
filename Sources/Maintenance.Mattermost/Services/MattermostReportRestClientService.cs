using Maintenance.Mattermost.Model;
using Maintenance.Mattermost.Params;

namespace Maintenance.Mattermost.Services;

public interface MattermostReportRestClientService
{
    List<MattermostRecord> GetLogs(GetLogParams queryParams);

    List<FilterTarget> GetTargets(GetLogParams queryParams);

    List<FilterGroup> GetGroupsByTarget(GetLogParams queryParams);
}

public class MattermostReportRestClientServiceImp(IHttpClientService clientService) : MattermostReportRestClientService
{
    public List<MattermostRecord> GetLogs(GetLogParams queryParams)
    {
        var result = clientService.PostAsync<List<MattermostRecord>, GetLogParams>("GetLogs", queryParams);
        return result;
    }

    public List<FilterTarget> GetTargets(GetLogParams queryParams)
    {
        var result = clientService.PostAsync<List<FilterTarget>, GetLogParams>("GetTargets", queryParams);
        return result;
    }
    public List<FilterGroup> GetGroupsByTarget(GetLogParams queryParams)
    {
        var result = clientService.PostAsync<List<FilterGroup>, GetLogParams>("GetGroupsByTarget", queryParams);
        return result;
    }
}