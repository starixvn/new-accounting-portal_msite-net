namespace Maintenance.Sites.Controllers
{
    using Fanex.Data;
    using Fanex.NAP.Client.Attributes;
    using Microsoft.AspNetCore.Mvc;

    [SessionAuthorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClearFanexDataCache()
        {
            throw new ArgumentException("Test");
            var content = CacheManager.ClearCache(100).Result;
            return Content(content);
        }
    }
}