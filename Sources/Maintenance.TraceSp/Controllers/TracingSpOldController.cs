namespace Maintenance.TraceSp.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("ProfilerSP/TracingSp")]
public class TracingSpOldController : Controller
{
    [Route("Index")]
    public ActionResult Index()
    {
        return Redirect("/TracingSp");
    }

    [Route("")]
    public ActionResult None()
    {
        return Redirect("/TracingSp");
    }
}