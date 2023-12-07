namespace Maintenance.Letstalk.Services;

using System.Collections.Generic;
using Model;
using Params;

public interface ILetstalkReportRestClientService
{
    List<FilterTarget> GetTargets(GetLogParams queryParams);

    List<FilterGroup> GetGroupsByTarget(GetLogParams queryParams);

    List<LetstalkRecord> GetLogs(GetLogParams queryParams);

    bool ReSendMail(ResendParams resendParams);

    bool ReSendMessage(ResendParams resendParams);
}

public class LetstalkReportRestClientService(IHttpClientService clientService) : ILetstalkReportRestClientService
{
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

    public List<LetstalkRecord> GetLogs(GetLogParams queryParams)
    {
        var result = clientService.PostAsync<List<LetstalkRecord>, GetLogParams>("GetLogs", queryParams);
        return result;
    }

    public bool ReSendMail(ResendParams resendParams)
    {
        var result = clientService.PostAsync<bool, ResendParams>("ReSendMail", resendParams);
        return result;
    }

    public bool ReSendMessage(ResendParams resendParams)
    {
        var result = clientService.PostAsync<bool, ResendParams>("ReSendMessage", resendParams);
        return result;
    }
}