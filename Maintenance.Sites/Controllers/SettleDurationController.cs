namespace Maintenance.Sites.Controllers;

using Fanex.NAP.Client.Attributes;
using Maintenance.Sites.Services;
using Microsoft.AspNetCore.Mvc;

[SessionAuthorize]
public class SettleDurationController(SettleDurationService service) : Controller
{
    public async Task<ActionResult> Index()
    {
        var models = await service.Query(DateTimeOffset.UtcNow.AddMonths(-1).Date, DateTimeOffset.UtcNow.Date);

        await service.SendLetstalk();

        return View(models);
    }
}