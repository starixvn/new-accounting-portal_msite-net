namespace Maintenance.TraceSp.Controllers
{
    using System.Linq;
    using Fanex.NAP.Client.Attributes;
    using Fanex.NAP.Client.Helpers.Business;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Services;

    public class TracingSpController(
        IDbServerService dbServerService,
        ITracingSpService tracingSpService, 
        ISiteService siteService,
        IConfiguration configuration) : Controller
    {
        [NapAuth(FunctionKey = SiteFunctionKeys.TracingSP, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var dbServerXmlPath = configuration.GetValue<string>("TracingSp:DbListXMLPath");
            var siteXmlPath = configuration.GetValue<string>("TracingSp:SiteXMLPath");
            var servers = dbServerService.GetDbServerList(dbServerXmlPath);
            var sites = siteService.GetSites(siteXmlPath);
            var model = new TracingSp
            {
                Servers = servers.Select(x => new Server { Ip = x.Ip, Name = x.Name + " (" + x.Ip + ")" }).ToList(),
                Sites = sites
            };

            return View("~/Areas/TracingSp/Index.cshtml", model);
        }

        [HttpPost]
        //[NapAuth(FunctionKey = SiteFunctionKeys.TracingSP, Policy = Constant.Policy.Read)]
        public ActionResult GetTracingSpData(TracingSp criteria)
        {
            var model = new TracingSp
            {
                Mode = false,
                TraceStartEvents = criteria.TraceStartEvents,
                Server = criteria.Server,
                Site = criteria.Site,
                TextDataLike = AddPercentSign(criteria.TextDataLike),
                TextDataLike2 = AddPercentSign(criteria.TextDataLike2),
                TextDataLike3 = AddPercentSign(criteria.TextDataLike3),
                TraceTimer = criteria.TraceTimer,
                ErrorMsg = string.Empty,
                HasError = false
            };

            var tracingSpData = tracingSpService.GetTracingSpData(model);

            return Json(new
            {
                Data = tracingSpData,
                model.HasError,
                model.ErrorMsg
            });
        }

        private static string AddPercentSign(string textLike)
        => !string.IsNullOrWhiteSpace(textLike) && !textLike.Contains("%") ? $"%{textLike}%" : textLike;
    }
}
