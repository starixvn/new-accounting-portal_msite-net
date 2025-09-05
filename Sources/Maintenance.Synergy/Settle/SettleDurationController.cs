using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maintenance.Synergy.Settle;

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

    public async Task<ActionResult> Adhoc(string date = "")
    {
        await service.Send(date);
        return Ok("ok");
    }


    private static string[] ModelToString(SettleDurationModel model)
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