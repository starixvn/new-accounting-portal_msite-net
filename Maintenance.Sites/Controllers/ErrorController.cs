namespace Maintenance.Sites.Controllers;

using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[AllowAnonymous]
[Route("Error")]
public class ErrorController : Controller
{
    [Route("{statusCode:int}")]
    public ActionResult Index(int statusCode)
    {
        if (statusCode == (int)HttpStatusCode.NotFound)
        {
            return Redirect("/");
        }

        ViewBag.ErrorMessage = $"Status code: {statusCode}.";

        return View();
    }
}