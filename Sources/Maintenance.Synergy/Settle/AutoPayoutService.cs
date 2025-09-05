using Epplus.Mapper.Extensions;
using Fanex.Data.Repository;
using Fanex.LetstalkService.Client;
using Fanex.LetstalkService.Client.Models;
using Fanex.RocketChat.Client;
using Fanex.RocketChat.Client.Models;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

namespace Maintenance.Synergy.Settle;

public class AutoPayoutService(
    IDynamicRepository repository,
    IConfiguration configuration,
    RocketChatClientService rocketChatClientService,
    LetstalkClientService letstalkService)
{
    public async Task<bool> Send(string dateInString = "")
    {
        var environment = configuration.GetValue<string>("RocketChat:Environment");
        if (string.IsNullOrEmpty(environment) || !environment.Contains("PROD"))
        {
            return false;
        }

        var date = string.IsNullOrEmpty(dateInString)
        ? DateTimeOffset.UtcNow.Date.AddDays(-1)
        : DateTime.ParseExact(dateInString, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

        var (details, summary, soccer, basketball) = await Query();

        var message = GenerateMessage(environment, summary, soccer, basketball);

        var groupName = configuration.GetValue<string>("Letstalk:RichardGroupName");
        var letstalkModel = new LetstalkModel
        {
            Content = message,
            Group = groupName
        };

        letstalkService.SendMessageByBotApi(letstalkModel);

        var excelBytes = await GenerateAutoPayoutDetail(details);

        var messageRequest = new MessageRequest
        {
            ChannelName = "SynergyKPI",
            Message = message,
            Files =
            [
                new FileModel
                {
                    FileName = $"AutoPayoutDetail-{date:yyyyMMdd}.xlsx",
                    FileBytes = excelBytes,
                    MediaType = MediaType.Excel
                }
            ]
        };

        var result = await rocketChatClientService.Send(messageRequest);

        return result;
    }

    private string GenerateMessage(
        string environment,
        IEnumerable<AutoPayoutRate> summary,
        IEnumerable<AutoPayoutRate> soccer,
        IEnumerable<AutoPayoutRate> basketball)
    {
        var date = DateTimeOffset.UtcNow.Date.AddDays(-1);

        var total = summary.Sum(m => m.AutoCalculate + m.AutoRefund + m.ManualCalculate + m.ManualRefund);
        var auto = summary.Sum(m => m.AutoCalculate + m.AutoRefund);
        var rate = total == 0 ? "NA" : $"{(double)auto * 100 / total:N2}%";

        var realTotal = summary.Where(m => m.LeagueType == "Real").Sum(m => m.AutoCalculate + m.AutoRefund + m.ManualCalculate + m.ManualRefund);
        var realAuto = summary.Where(m => m.LeagueType == "Real").Sum(m => m.AutoCalculate + m.AutoRefund);
        var realRate = realTotal == 0 ? "NA" : $"{(double)realAuto * 100 / realTotal:N2}%";

        var virtualTotal = summary.Where(m => m.LeagueType == "Virtual").Sum(m => m.AutoCalculate + m.AutoRefund + m.ManualCalculate + m.ManualRefund);
        var virtualAuto = summary.Where(m => m.LeagueType == "Virtual").Sum(m => m.AutoCalculate + m.AutoRefund);
        var virtualRate = virtualTotal == 0 ? "NA" : $"{(double)virtualAuto * 100 / virtualTotal:N2}%";

        var soccerTotal = soccer.Sum(m => m.AutoCalculate + m.AutoRefund + m.ManualCalculate + m.ManualRefund);
        var soccerAuto = soccer.Sum(m => m.AutoCalculate + m.AutoRefund);
        var soccerRate = soccerTotal == 0 ? "NA" : $"{(double)soccerAuto * 100 / soccerTotal:N2}%";

        var soccerRealTotal = soccer.Where(m => m.LeagueType == "Real").Sum(m => m.AutoCalculate + m.AutoRefund + m.ManualCalculate + m.ManualRefund);
        var soccerRealAuto = soccer.Where(m => m.LeagueType == "Real").Sum(m => m.AutoCalculate + m.AutoRefund);
        var soccerRealRate = soccerRealTotal == 0 ? "NA" : $"{(double)soccerRealAuto * 100 / soccerRealTotal:N2}%";

        var soccerVirtualTotal = soccer.Where(m => m.LeagueType == "Virtual").Sum(m => m.AutoCalculate + m.AutoRefund + m.ManualCalculate + m.ManualRefund);
        var soccerVirtualAuto = soccer.Where(m => m.LeagueType == "Virtual").Sum(m => m.AutoCalculate + m.AutoRefund);
        var soccerVirtualRate = soccerVirtualTotal == 0 ? "NA" : $"{(double)soccerVirtualAuto * 100 / soccerVirtualTotal:N2}%";

        var basketballTotal = basketball.Sum(m => m.AutoCalculate + m.AutoRefund + m.ManualCalculate + m.ManualRefund);
        var basketballAuto = basketball.Sum(m => m.AutoCalculate + m.AutoRefund);
        var basketballRate = basketballTotal == 0 ? "NA" : $"{(double)basketballAuto * 100 / basketballTotal:N2}%";

        var basketballRealTotal = basketball.Where(m => m.LeagueType == "Real").Sum(m => m.AutoCalculate + m.AutoRefund + m.ManualCalculate + m.ManualRefund);
        var basketballRealAuto = basketball.Where(m => m.LeagueType == "Real").Sum(m => m.AutoCalculate + m.AutoRefund);
        var basketballRealRate = basketballRealTotal == 0 ? "NA" : $"{(double)basketballRealAuto * 100 / basketballRealTotal:N2}%";

        var basketballVirtualTotal = basketball.Where(m => m.LeagueType == "Virtual").Sum(m => m.AutoCalculate + m.AutoRefund + m.ManualCalculate + m.ManualRefund);
        var basketballVirtualAuto = basketball.Where(m => m.LeagueType == "Virtual").Sum(m => m.AutoCalculate + m.AutoRefund);
        var basketballVirtualRate = basketballVirtualTotal == 0 ? "NA" : $"{(double)basketballVirtualAuto * 100 / basketballVirtualTotal:N2}%";

        var message = $""""
                       Date {date} ({environment}): 
                       1. All Sports: {rate} ({auto:N0} / {total:N0})
                       - Real: {realRate} ({realAuto:N0} / {realTotal:N0})
                       - Virtual: {virtualRate} ({virtualAuto:N0} / {virtualTotal:N0})
                       2. Soccer: {soccerRate} ({soccerAuto:N0} / {soccerTotal:N0})
                       - Real: {soccerRealRate} ({soccerRealAuto:N0} / {soccerRealTotal:N0})
                       - Virtual: {soccerVirtualRate} ({soccerVirtualAuto:N0} / {soccerVirtualTotal:N0})
                       3. Basketball: {basketballRate} ({basketballAuto:N0} / {basketballTotal:N0})
                       - Real: {basketballRealRate} ({basketballRealAuto:N0} / {basketballRealTotal:N0})
                       - Virtual: {basketballVirtualRate} ({basketballVirtualAuto:N0} / {basketballVirtualTotal:N0})
                       """";
        return message;
    }

    public async Task<byte[]> GenerateAutoPayoutDetail(IEnumerable<AutoPayoutRateDetail> details)
    {
        const string templateAbsolutelyPath = "App_Data/Templates/SettleAutoPayout.xlsx";
        await using var stream = new FileStream(templateAbsolutelyPath, FileMode.Open, FileAccess.Read);
        using var excelPackage = new ExcelPackage(stream);
        var detailSheet = excelPackage.Workbook.Worksheets["Detail"];

        var date = DateTimeOffset.UtcNow.Date.AddDays(-1);
        detailSheet.Cells["A1"].Value = date.ToShortDateString();
        detailSheet.ApplyVertical(details, 3);

        if (!details.Any())
        {
            detailSheet.DeleteRow(3);
        }

        return await excelPackage.GetAsByteArrayAsync();
    }

    public async Task<(List<AutoPayoutRateDetail> details, List<AutoPayoutRate> summary, List<AutoPayoutRate> soccer, List<AutoPayoutRate> basketball)> Query()
    {
        var date = DateTimeOffset.UtcNow.Date.AddDays(-1);
        var criteria = new AutoPayoutRateCriteria
        {
            FromDate = date,
            ToDate = date
        };
        var (a, b, c, d) = await repository.FetchMultipleAsync<AutoPayoutRateDetail, AutoPayoutRate, AutoPayoutRate, AutoPayoutRate>(criteria);
        return (a.ToList(), b.ToList(), c.ToList(), d.ToList());
    }
}