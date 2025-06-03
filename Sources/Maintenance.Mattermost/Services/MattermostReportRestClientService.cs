using Fanex.Mattermost.Client;
using Fanex.Mattermost.Client.Models;
using Maintenance.Mattermost.Constants;
using Microsoft.Extensions.Configuration;

namespace Maintenance.Mattermost.Services;

public interface MattermostReportRestClientService
{
    Task<List<MattermostRecord>> GetLogs(QueryParams queryParams);

    Task<List<FilterTarget>> GetTargets(QueryParams queryParams);

    Task<List<FilterGroup>> GetGroupsByTarget(QueryParams queryParams);

    Task<bool> ReSendMessage(QueryParams resendParams);

    Task<bool> ReSendMail(QueryParams resendParams);
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
        return (await service.GetTargets(queryParams)).Where(target => target.TargetName == CommonConstants.Mattermost).ToList();
    }
    public async Task<List<FilterGroup>> GetGroupsByTarget(QueryParams queryParams)
    {
        return await service.GetGroupsByTarget(queryParams);
    }

    public async Task<bool> ReSendMessage(QueryParams resendParams)
    {
        return await service.ReSend(resendParams.Id);
    }

    public async Task<bool> ReSendMail(QueryParams resendParams)
    {
        return await service.ReSend(resendParams.Id);
    }
}