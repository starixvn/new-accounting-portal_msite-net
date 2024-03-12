namespace Maintenance.Letstalk.Controllers;

using Fanex.NAP.Client.Attributes;
using Microsoft.AspNetCore.Mvc;

[NapSuperAuth]
[Route("LetstalkNotification/ManageLetstalkMessage")]
public class ManageLetstalkMessageOldController() : Controller
{
    [Route("Index")]
    public ActionResult Index()
    {
        return Redirect("/ManageLetstalkMessage");
    }

    [Route("")]
    public ActionResult None()
    {
        return Redirect("/ManageLetstalkMessage");
    }
}