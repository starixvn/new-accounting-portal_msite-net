namespace Maintenance.Letstalk.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("LetstalkNotification/LetstalkReport")]
public class LetstalkReportOldController() : Controller
{
    [Route("Index")]
    public ActionResult Index()
    {
        return Redirect("/LetstalkReport");
    }

    [Route("")]
    public ActionResult None()
    {
        return Redirect("/LetstalkReport");
    }
}