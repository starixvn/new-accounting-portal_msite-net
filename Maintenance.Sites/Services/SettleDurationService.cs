using Fanex.Data.Repository;
using Fanex.LetstalkService.Client;
using Fanex.LetstalkService.Client.Models;

namespace Maintenance.Sites.Services;

public class SettleDurationService(
    IDynamicRepository repository, 
    IConfiguration configuration,
    LetstalkClientService letstalkService)
{
    public class SettleDurationModel
    {
        public DateTime Date { get; set; }

        public string SportName { get; set; }

        public int MatchCount { get; set; }

        public int MatchCountAction { get; set; }

        public long SettleDuration { get; set; }
    }

    public class SettleDurationCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey() => "NAP_Set_SettleDurationReport";

        public override bool IsValid() => true;
    }

    public async Task<List<SettleDurationModel>> Query(DateTime fromDate, DateTime toDate)
    {
        var models = await repository.FetchAsync<SettleDurationModel>(new SettleDurationCriteria
        {
            FromDate = fromDate,
            ToDate = toDate
        });
        
        return models.ToList();
    }

    public async Task<bool> SendLetstalk()
    {
        const string template = """
                                {environment} Date {date}: 
                                1. Soccer:
                                - Total Match Count: {soccerMatchCount}
                                - Total Match Count Action: {soccerMatchCountAction}
                                - Average Duration Time: {soccerDuration}s
                                2. Basketball
                                - Total Match Count: {basketballMatchCount}
                                - Total Match Count Action: {basketballMatchCountAction}
                                - Average Duration Time: {basketballDuration}s
                                """;
        var environment = configuration.GetValue<string>("Letstalk:Environment");
        if (string.IsNullOrEmpty(environment) || !environment.Contains("PROD"))
        {
            return false;
        }

        var date = DateTimeOffset.UtcNow.Date.AddDays(-1);
        var models = await Query(date, date);
        
        string message;
        if (!models.Any())
        {
            message =
                $@"{environment} - No settle duration data found for date {DateTimeOffset.UtcNow.Date.AddDays(-1).ToShortDateString()}.";
        }
        else
        {
            var soccerModel = models
                .Find(m => "Soccer".Equals(m.SportName, StringComparison.CurrentCultureIgnoreCase));
            var basketballModel = models
                .Find(m => "Basketball".Equals(m.SportName, StringComparison.CurrentCultureIgnoreCase));
            var soccerMatchCount = soccerModel == null ? "NA" : soccerModel.MatchCount.ToString();
            var soccerMatchCountAction = soccerModel == null ? "NA" : soccerModel.MatchCountAction.ToString();
            var soccerDuration = soccerModel == null ? "NA" : ((decimal)soccerModel.SettleDuration / 1000).ToString("#,##0.00");
            var basketballMatchCount = basketballModel == null ? "NA" : basketballModel.MatchCount.ToString();
            var basketballDuration = basketballModel == null ? "NA" : ((decimal)basketballModel.SettleDuration / 1000).ToString("#,##0.00");
            var basketballMatchCountAction = basketballModel == null ? "NA" : basketballModel.MatchCountAction.ToString();
            message = template.Replace("{environment}", environment)
                .Replace("{date}", date.ToShortDateString())
                .Replace("{soccerMatchCount}", soccerMatchCount)
                .Replace("{soccerMatchCountAction}", soccerMatchCountAction)
                .Replace("{soccerDuration}", soccerDuration)
                .Replace("{basketballMatchCount}", basketballMatchCount)
                .Replace("{basketballMatchCountAction}", basketballMatchCountAction)
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