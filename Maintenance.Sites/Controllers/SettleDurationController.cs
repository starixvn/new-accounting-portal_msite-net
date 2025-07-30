using Microsoft.AspNetCore.Authorization;

namespace Maintenance.Sites.Controllers;

using Maintenance.Sites.Services;
using Microsoft.AspNetCore.Mvc;

[AllowAnonymous]
public class SettleDurationController(SettleDurationService service) : Controller
{
    public async Task<ActionResult> Index()
    {
        var models = await service.Query(DateTimeOffset.UtcNow.AddMonths(-1).Date, DateTimeOffset.UtcNow.Date);
        return View(models);
    }

    public async Task<ActionResult> Yesterday()
    {
        var yesterday = DateTimeOffset.UtcNow.Date.AddDays(-1);
        var models = await service.Query(yesterday, yesterday);
        var lines = models.Select(ModelToString);

        return Json(lines);
    }

    private static string[] ModelToString(SettleDurationService.SettleDurationModel model)
    {
        return
        [
            model.Date.ToShortDateString(),
            model.SportName,
            model.MatchCount.ToString(),
            model.MatchCountAction.ToString(),
            model.SettleDuration.ToString()
        ];
    }
}