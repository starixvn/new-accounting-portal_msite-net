using Fanex.LetstalkService.Client.Models;

namespace Maintenance.Sites.Controllers;

using Fanex.Data.Repository;
using Fanex.LetstalkService.Client;
using Fanex.NAP.Client.Attributes;
using Microsoft.AspNetCore.Mvc;

[SessionAuthorize]
public class SettleDurationController(IDynamicRepository repository, IConfiguration configuration, LetstalkClientService letstalkService) : Controller
{
    public class SettleDurationModel
    {
       public DateTime Date { get; set; }
       
       public string SportName { get; set; }

       public int MatchCount { get; set; }
       public long SettleDuration { get; set; }
    }

    public class SettleDurationCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey() => "NAP_Set_SettleDurationReport";

        public override bool IsValid() => true;
    }

    public async Task<ActionResult> Index()
    {
        var models = await repository.FetchAsync<SettleDurationModel>(new SettleDurationCriteria
        {
            FromDate = DateTimeOffset.UtcNow.AddMonths(-1).Date,
            ToDate = DateTimeOffset.UtcNow.Date
        });

        await SendLetstalk();

        return View(models);
    }

    public async Task<bool> SendLetstalk()
    {
        const string template = """
                                Date {date}: 
                                1. Soccer:
                                - Total Match Count: {soccerMatchCount}
                                - Average Duration Time: {soccerDuration}s
                                2. Basketball
                                - Total Match Count: {basketballMatchCount}
                                - Average Duration Time: {basketballDuration}s
                                """;
        var date = DateTimeOffset.UtcNow.Date.AddDays(-1);
        var models = (await repository.FetchAsync<SettleDurationModel>(new SettleDurationCriteria
        {
            FromDate = date,
            ToDate = date
        })).ToList();

        string message;
        if (!models.Any())
        {
            message =
                $@"No settle duration data found for date {DateTimeOffset.UtcNow.Date.AddDays(-1).ToShortDateString()}.";
        }
        else
        {
            var soccerModel = models
                .FirstOrDefault(m => "Soccer".Equals(m.SportName, StringComparison.CurrentCultureIgnoreCase));
            var basketballModel = models
                .FirstOrDefault(m => "Basketball".Equals(m.SportName, StringComparison.CurrentCultureIgnoreCase));
            var soccerMatchCount = soccerModel == null ? "NA" : soccerModel.MatchCount.ToString();
            var soccerDuration = soccerModel == null ? "NA" : ((decimal)soccerModel.SettleDuration / 1000).ToString("#,##0.00");
            var basketballMatchCount = basketballModel == null ? "NA" : basketballModel.MatchCount.ToString();
            var basketballDuration = basketballModel == null ? "NA" : ((decimal)basketballModel.SettleDuration / 1000).ToString("#,##0.00");
            message = template.Replace("{date}", date.ToShortDateString())
                .Replace("{soccerMatchCount}", soccerMatchCount)
                .Replace("{soccerDuration}", soccerDuration)
                .Replace("{basketballMatchCount}", basketballMatchCount)
                .Replace("{basketballDuration}", basketballDuration);
        }

        var groupName = configuration.GetValue<string>("Letstalk:RichardGroupName");
        var letstalkModel = new LetstalkModel
        {
            Content = message,
            Group = groupName
        };

        var result = letstalkService.SendMessageByBotApi(letstalkModel);

        return result;
    }
}