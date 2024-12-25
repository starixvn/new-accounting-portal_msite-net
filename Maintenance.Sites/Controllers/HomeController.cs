namespace Maintenance.Sites.Controllers;

using Fanex.NAP.Client.Attributes;
using Microsoft.AspNetCore.Mvc;
[SessionAuthorize]
public class HomeController() : Controller
{
    public ActionResult Index()
    {
        return View();
    }
}