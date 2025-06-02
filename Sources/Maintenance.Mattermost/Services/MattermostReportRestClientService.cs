using Fanex.Mattermost.Client;
using Fanex.Mattermost.Client.Models;
using Microsoft.Extensions.Configuration;

namespace Maintenance.Mattermost.Services;

public interface MattermostReportRestClientService
{
    Task<List<MattermostRecord>> GetLogs(QueryParams queryParams);

    Task<List<FilterTarget>> GetTargets(QueryParams queryParams);

    Task<List<FilterGroup>> GetGroupsByTarget(QueryParams queryParams);
}

public class MattermostReportRestClientServiceImp : MattermostReportRestClientService
{
    private readonly MattermostAdminService service;

    public MattermostReportRestClientServiceImp(MattermostAdminService service, IConfiguration configuration)
    {
        this.service = service;
        AppConfigService.Config.MattermostServiceApi = configuration.GetValue<string>("Mattermost:MattermostApiUrl");
    }
    public async Task<List<MattermostRecord>> GetLogs(QueryParams queryParams)
    {
        var result = await service.GetLogs(queryParams);
        return result;
    }

    public async Task<List<FilterTarget>> GetTargets(QueryParams queryParams)
    {
        return await service.GetTargets(queryParams);
    }
    public async Task<List<FilterGroup>> GetGroupsByTarget(QueryParams queryParams)
    {
        return await service.GetGroupsByTarget(queryParams);
    }
}