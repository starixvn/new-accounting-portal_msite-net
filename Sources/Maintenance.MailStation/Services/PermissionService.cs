namespace Maintenance.MailStation.Services;

using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Business;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Client.Services;

public class PermissionService(INapSession currentSession, INapSettings napSettings, IUserService userService)
{
    public bool HasReadPermission(string functionName)
    {
        return HasPermission(functionName);
    }

    public bool HasHostPermission(string functionName)
    {
        return HasPermission(functionName, Constant.Policy.Host);
    }

    public  bool HasPermission(string functionName, Constant.Policy functionPolicy = Constant.Policy.Read)
    {
        return userService.IsHavingPermission(
            currentSession.UserId,
            napSettings.SiteId,
            functionName,
            functionPolicy);
    }
}