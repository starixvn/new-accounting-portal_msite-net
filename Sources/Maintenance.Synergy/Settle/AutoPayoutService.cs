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
    public async Task<bool> Send()
    {
        var environment = configuration.GetValue<string>("RocketChat:Environment");
        if (string.IsNullOrEmpty(environment) || !environment.Contains("PROD"))
        {
            return false;
        }

        var (autoPayoutModels, soccerAutoPayoutModels, basketballAutoPayoutModels) = await Query();

        var message = GenerateMessage(environment, autoPayoutModels, soccerAutoPayoutModels, basketballAutoPayoutModels);

        var groupName = configuration.GetValue<string>("Letstalk:RichardGroupName");
        var letstalkModel = new LetstalkModel
        {
            Content = message,
            Group = groupName
        };

        letstalkService.SendMessageByBotApi(letstalkModel);

        var date = DateTimeOffset.UtcNow.Date.AddDays(-1);
        var excelBytes = await GenerateAutoPayoutDetail(autoPayoutModels, soccerAutoPayoutModels, basketballAutoPayoutModels);

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
        IEnumerable<AutoPayoutRate> autoPayoutModels,
        IEnumerable<SportAutoPayoutRate> soccerAutoPayoutModels,
        IEnumerable<SportAutoPayoutRate> basketballAutoPayoutModels)
    {
        var date = DateTimeOffset.UtcNow.Date.AddDays(-1);

        var allSportsTotal = autoPayoutModels.Sum(m => m.AutoCalculate + m.AutoRefund + m.ManualCalculate + m.ManualRefund);
        var allSportsAuto = autoPayoutModels.Sum(m => m.AutoCalculate + m.AutoRefund);
        var allSportsRate = allSportsTotal == 0 ? "NA" : $"{(double)allSportsAuto * 100 / allSportsTotal:N2}%";

        var allSportsRealTotal = autoPayoutModels.Where(m => m.LeagueType == "Real").Sum(m => m.AutoCalculate + m.AutoRefund + m.ManualCalculate + m.ManualRefund);
        var allSportsRealAuto = autoPayoutModels.Where(m => m.LeagueType == "Real").Sum(m => m.AutoCalculate + m.AutoRefund);
        var allSportsRealRate = allSportsRealTotal == 0 ? "NA" : $"{(double)allSportsRealAuto * 100 / allSportsRealTotal:N2}%";

        var allSportsVirtualTotal = autoPayoutModels.Where(m => m.LeagueType == "Virtual").Sum(m => m.AutoCalculate + m.AutoRefund + m.ManualCalculate + m.ManualRefund);
        var allSportsVirtualAuto = autoPayoutModels.Where(m => m.LeagueType == "Virtual").Sum(m => m.AutoCalculate + m.AutoRefund);
        var allSportsVirtualRate = allSportsVirtualTotal == 0 ? "NA" : $"{(double)allSportsVirtualAuto * 100 / allSportsVirtualTotal:N2}%";

        var soccerTotal = soccerAutoPayoutModels.Sum(m => m.AutoCalculate + m.AutoRefund + m.ManualCalculate + m.ManualRefund);
        var soccerAuto = soccerAutoPayoutModels.Sum(m => m.AutoCalculate + m.AutoRefund);
        var soccerRate = soccerTotal == 0 ? "NA" : $"{(double)soccerAuto * 100 / soccerTotal:N2}%";

        var soccerRealTotal = soccerAutoPayoutModels.Where(m => m.LeagueType == "Real").Sum(m => m.AutoCalculate + m.AutoRefund + m.ManualCalculate + m.ManualRefund);
        var soccerRealAuto = soccerAutoPayoutModels.Where(m => m.LeagueType == "Real").Sum(m => m.AutoCalculate + m.AutoRefund);
        var soccerRealRate = soccerRealTotal == 0 ? "NA" : $"{(double)soccerRealAuto * 100 / soccerRealTotal:N2}%";

        var soccerVirtualTotal = soccerAutoPayoutModels.Where(m => m.LeagueType == "Virtual").Sum(m => m.AutoCalculate + m.AutoRefund + m.ManualCalculate + m.ManualRefund);
        var soccerVirtualAuto = soccerAutoPayoutModels.Where(m => m.LeagueType == "Virtual").Sum(m => m.AutoCalculate + m.AutoRefund);
        var soccerVirtualRate = soccerVirtualTotal == 0 ? "NA" : $"{(double)soccerVirtualAuto * 100 / soccerVirtualTotal:N2}%";

        var basketballTotal = basketballAutoPayoutModels.Sum(m => m.AutoCalculate + m.AutoRefund + m.ManualCalculate + m.ManualRefund);
        var basketballAuto = basketballAutoPayoutModels.Sum(m => m.AutoCalculate + m.AutoRefund);
        var basketballRate = basketballTotal == 0 ? "NA" : $"{(double)basketballAuto * 100 / basketballTotal:N2}%";

        var basketballRealTotal = basketballAutoPayoutModels.Where(m => m.LeagueType == "Real").Sum(m => m.AutoCalculate + m.AutoRefund + m.ManualCalculate + m.ManualRefund);
        var basketballRealAuto = basketballAutoPayoutModels.Where(m => m.LeagueType == "Real").Sum(m => m.AutoCalculate + m.AutoRefund);
        var basketballRealRate = basketballRealTotal == 0 ? "NA" : $"{(double)basketballRealAuto * 100 / basketballRealTotal:N2}%";

        var basketballVirtualTotal = basketballAutoPayoutModels.Where(m => m.LeagueType == "Virtual").Sum(m => m.AutoCalculate + m.AutoRefund + m.ManualCalculate + m.ManualRefund);
        var basketballVirtualAuto = basketballAutoPayoutModels.Where(m => m.LeagueType == "Virtual").Sum(m => m.AutoCalculate + m.AutoRefund);
        var basketballVirtualRate = basketballVirtualTotal == 0 ? "NA" : $"{(double)basketballVirtualAuto * 100 / basketballVirtualTotal:N2}%";

        var message = $""""
                       Date {date} ({environment}): 
                       1. All Sports: {allSportsRate} ({allSportsAuto:N0} / {allSportsTotal:N0})
                       - Real: {allSportsRealRate} ({allSportsRealAuto:N0} / {allSportsRealTotal:N0})
                       - Virtual: {allSportsVirtualRate} ({allSportsVirtualAuto:N0} / {allSportsVirtualTotal:N0})
                       2. Soccer: {soccerRate} ({soccerAuto:N0} / {soccerTotal:N0})
                       - Real: {soccerRealRate} ({soccerRealAuto:N0} / {soccerRealTotal:N0})
                       - Virtual: {soccerVirtualRate} ({soccerVirtualAuto:N0} / {soccerVirtualTotal:N0})
                       3. Basketball: {basketballRate} ({basketballAuto:N0} / {basketballTotal:N0})
                       - Real: {basketballRealRate} ({basketballRealAuto:N0} / {basketballRealTotal:N0})
                       - Virtual: {basketballVirtualRate} ({basketballVirtualAuto:N0} / {basketballVirtualTotal:N0})
                       """";
        return message;
    }

    public async Task<byte[]> GenerateAutoPayoutDetail(
        IEnumerable<AutoPayoutRate> autoPayoutModels,
        IEnumerable<SportAutoPayoutRate> soccerAutoPayoutModels,
        IEnumerable<SportAutoPayoutRate> basketballAutoPayoutModels)
    {
        const string templateAbsolutelyPath = "App_Data/Templates/SettleAutoPayout.xlsx";
        await using var stream = new FileStream(templateAbsolutelyPath, FileMode.Open, FileAccess.Read);
        using var excelPackage = new ExcelPackage(stream);
        var summarySheet = excelPackage.Workbook.Worksheets["Summary"];
        var soccerSheet = excelPackage.Workbook.Worksheets["Soccer"];
        var basketballSheet = excelPackage.Workbook.Worksheets["Basketball"];

        var date = DateTimeOffset.UtcNow.Date.AddDays(-1);
        summarySheet.Cells["A1"].Value = date.ToShortDateString();
        soccerSheet.Cells["A1"].Value = date.ToShortDateString();
        basketballSheet.Cells["A1"].Value = date.ToShortDateString();
        summarySheet.ApplyVertical(autoPayoutModels, 3);
        soccerSheet.ApplyVertical(soccerAutoPayoutModels, 3);
        basketballSheet.ApplyVertical(basketballAutoPayoutModels, 3);

        if (!autoPayoutModels.Any())
        {
            summarySheet.DeleteRow(3);
        }
        if (!soccerAutoPayoutModels.Any())
        {
            soccerSheet.DeleteRow(3);
        }
        if (!basketballAutoPayoutModels.Any())
        {
            basketballSheet.DeleteRow(3);
        }

        return await excelPackage.GetAsByteArrayAsync();
    }

    public async Task<(
        IEnumerable<AutoPayoutRate> payout,
        IEnumerable<SportAutoPayoutRate> soccerPayout,
        IEnumerable<SportAutoPayoutRate> basketballPayout)> Query()
    {
        var date = DateTimeOffset.UtcNow.Date.AddDays(-1);
        var criteria = new AutoPayoutRateCriteria
        {
            FromDate = date,
            ToDate = date
        };
        var (autoPayoutModels, soccerAutoPayoutModels, basketballAutoPayoutModels) = await repository.FetchMultipleAsync<AutoPayoutRate, SportAutoPayoutRate, SportAutoPayoutRate>(criteria);
        return (autoPayoutModels, soccerAutoPayoutModels, basketballAutoPayoutModels);
    }
}