using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maintenance.Synergy.Settle;

[AllowAnonymous]
public class AutoPayoutController(AutoPayoutService service) : Controller
{
    public async Task<ActionResult> Index()
    {
        await service.Send();
        return Ok("ok");
    }

    public async Task<ActionResult> Adhoc(string date = "")
    {
        await service.Send(date);
        return Ok("ok");
    }
}