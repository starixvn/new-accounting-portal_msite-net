using Epplus.Mapper.Extensions;
using Fanex.Data.Repository;
using Fanex.LetstalkService.Client;
using Fanex.LetstalkService.Client.Models;
using Fanex.RocketChat.Client;
using Fanex.RocketChat.Client.Models;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

namespace Maintenance.Synergy.Settle;

public class SettleDurationService(
    IDynamicRepository repository, 
    IConfiguration configuration,
    LetstalkClientService letstalkService,
    RocketChatClientService rocketChatClientService)
{
    public async Task<List<SettleDurationModel>> Query(DateTime fromDate, DateTime toDate)
    {
        var models = await repository.FetchAsync<SettleDurationModel>(new SettleDurationCriteria
        {
            FromDate = fromDate,
            ToDate = toDate
        });
        
        return [.. models];
    }

    public async Task<bool> Send()
    {
        const string template = """
                                Date {date}({environment}): 
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
        var (models, details) = await repository.FetchMultipleAsync<SettleDurationModel, SettleDurationDetail>(new SettleDurationCriteria
        {
            FromDate = date,
            ToDate = date
        });
        
        var message = !models.Any()
            ? $"{environment} - No settle duration data found for date {DateTimeOffset.UtcNow.Date.AddDays(-1).ToShortDateString()}." 
            :GenerateNonEmptyMessage(models, template, environment, date);

        var groupName = configuration.GetValue<string>("Letstalk:RichardGroupName");
        var letstalkModel = new LetstalkModel
        {
            Content = message,
            Group = groupName
        };

        var result = letstalkService.SendMessageByBotApi(letstalkModel);

        var excelBytes = await GenerateSettleDurationDetail(details);

        var messageRequest = new MessageRequest
        {
            ChannelName = "SynergyKPI",
            Message = message,
            Files =
            [
                new FileModel
                {
                    FileName = $"SettleDurationDetail-{date:yyyyMMdd}.xlsx",
                    FileBytes = excelBytes,
                    MediaType = MediaType.Excel
                }
            ]
        };

        await rocketChatClientService.Send(messageRequest);


        return result;
    }

    private static string GenerateNonEmptyMessage(IEnumerable<SettleDurationModel> models, string template, string environment, DateTime date)
    {
        var soccerModel = models
            .FirstOrDefault(m => "Soccer".Equals(m.SportName, StringComparison.CurrentCultureIgnoreCase));
        var basketballModel = models
            .FirstOrDefault(m => "Basketball".Equals(m.SportName, StringComparison.CurrentCultureIgnoreCase));
        var soccerMatchCount = soccerModel == null ? "NA" : soccerModel.MatchCount.ToString();
        var soccerMatchCountAction = soccerModel == null ? "NA" : soccerModel.MatchCountAction.ToString();
        var soccerDuration = soccerModel == null ? "NA" : ((decimal)soccerModel.SettleDuration / 1000).ToString("#,##0.00");
        var basketballMatchCount = basketballModel == null ? "NA" : basketballModel.MatchCount.ToString();
        var basketballDuration = basketballModel == null ? "NA" : ((decimal)basketballModel.SettleDuration / 1000).ToString("#,##0.00");
        var basketballMatchCountAction = basketballModel == null ? "NA" : basketballModel.MatchCountAction.ToString();
        var message = template.Replace("{environment}", environment)
            .Replace("{date}", date.ToString("MM/dd/yyyy"))
            .Replace("{soccerMatchCount}", soccerMatchCount)
            .Replace("{soccerMatchCountAction}", soccerMatchCountAction)
            .Replace("{soccerDuration}", soccerDuration)
            .Replace("{basketballMatchCount}", basketballMatchCount)
            .Replace("{basketballMatchCountAction}", basketballMatchCountAction)
            .Replace("{basketballDuration}", basketballDuration);
        return message;
    }

    public async Task<byte[]> GenerateSettleDurationDetail(IEnumerable<SettleDurationDetail> models)
    {
        const string templateAbsolutelyPath = "App_Data/Templates/SettleDuration.xlsx";
        await using var stream = new FileStream(templateAbsolutelyPath, FileMode.Open, FileAccess.Read);
        using var excelPackage = new ExcelPackage(stream);
        var summarySheet = excelPackage.Workbook.Worksheets["Detail"];

        var date = DateTimeOffset.UtcNow.Date.AddDays(-1);
        summarySheet.Cells["A1"].Value = date.ToShortDateString();
        summarySheet.ApplyVertical(models, 3);

        if (!models.Any())
        {
            summarySheet.DeleteRow(3);
        }

        return await excelPackage.GetAsByteArrayAsync();
    }
}