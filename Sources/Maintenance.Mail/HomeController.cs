namespace Maintenance.Mail
{
    using Fanex.Data;
    using Fanex.NAP.Client.Attributes;
    using Fanex.NAP.Client.Helpers.Web;
    using Microsoft.AspNetCore.Mvc;

    [NapSuperAuth]
    public class HomeController(INapSession napSession ) : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _Menu()
        {
            ViewBag.Menu = napSession.MenuItemList;
            return PartialView("~/Views/Shared/_Menu.cshtml");
        }

        public ActionResult ClearFanexDataCache()
        {
            var content = CacheManager.ClearCache(100).Result;
            return Content(content);
        }
    }
}
