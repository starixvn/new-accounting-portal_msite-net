using Epplus.Mapper.Annotations;
using Fanex.Data.Repository;

namespace Maintenance.Synergy.Settle;

public class SettleDurationModel
{
    public DateTime Date { get; set; }

    public string SportName { get; set; }

    public int MatchCount { get; set; }

    public int MatchCountAction { get; set; }

    public long SettleDuration { get; set; }
}

public class SettleDurationDetail
{
    [Cell("A3")]
    public int Year { get; set; }

    [Cell("B3")]
    public int Month { get; set; }

    [Cell("C3")]
    public DateTime Date { get; set; }

    [Cell("D3")]
    public string Weekday { get; set; }

    [Cell("E3")]
    public int SportType { get; set; }

    [Cell("F3")]
    public string SportMame { get; set; }

    [Cell("G3")]
    public int LeagueGroupId { get; set; }

    [Cell("H3")]
    public string LeagueGroupName { get; set; }

    [Cell("I3")]
    public string LeagueName { get; set; }

    [Cell("J3")]
    public int MatchId { get; set; }

    [Cell("K3")]
    public string HomeTeam { get; set; }

    [Cell("L3")]
    public string AwayTeam { get; set; }

    [Cell("M3")]
    public DateTime GlobalShowTime { get; set; }

    [Cell("N3")]
    public string KeyScoreUser { get; set; }

    [Cell("O3")]
    public string SettleAction { get; set; }

    [Cell("P3")]
    public string SettlementStatus { get; set; }

    [Cell("Q3")]
    public string ConfirmStatus { get; set; }

    [Cell("R3")]
    public int Step1_WaitForKeyScore_Duration { get; set; }

    [Cell("S3")]
    public int Step2_WaitForSettle_Duration { get; set; }

    [Cell("T3")]
    public int Step3_Settle_Duration { get; set; }

    [Cell("U3")]
    public int IsHold { get; set; }

    [Cell("V3")]
    public bool IsLive { get; set; }

    [Cell("W3")]
    public bool IsChildMatch { get; set; }

    [Cell("X3")]
    public int Total { get; set; }
}


public class AutoPayoutRate
{
    [Cell("A3")]
    public DateTime EventDate { get; set; }
    [Cell("B3")]
    public string LeagueType { get; set; }
    [Cell("C3")]
    public int ManualCalculate { get; set; }
    [Cell("D3")]
    public int AutoCalculate { get; set; }
    [Cell("E3")]
    public int ManualRefund { get; set; }
    [Cell("F3")]
    public int AutoRefund { get; set; }
    [Cell("G3")]
    public double AutoRate
    {
        get
        {
            var total = ManualCalculate + AutoCalculate + ManualRefund + AutoRefund;
            var autoTotal = AutoCalculate + AutoRefund;
            return total == 0 ? 0 : (double)autoTotal / total;
        }
    }
}


public record AutoPayoutRateRecord
{
    [Cell("A3")]
    public DateTime EventDate { get; init; } 

    [Cell("B3")]
    public string LeagueType { get; init; }

    [Cell("C3")]
    public int ManualCalculate { get; init; }

    [Cell("D3")]
    public int AutoCalculate { get; init; }

    [Cell("E3")]
    public int ManualRefund { get; init; }

    [Cell("F3")]
    public int AutoRefund { get; init; }

    [Cell("G3")]
    public double AutoRate
    {
        get
        {
            var total = ManualCalculate + AutoCalculate + ManualRefund + AutoRefund;
            var autoTotal = AutoCalculate + AutoRefund;
            return total == 0 ? 0 : (double)autoTotal / total;
        }
    }
}


public class SportAutoPayoutRate
{
    [Cell("A3")]
    public DateTime EventDate { get; set; }
    [Cell("B3")]
    public string SportName { get; set; }
    [Cell("C3")]
    public string LeagueType { get; set; }
    [Cell("D3")]
    public int ManualCalculate { get; set; }
    [Cell("E3")]
    public int AutoCalculate { get; set; }
    [Cell("F3")]
    public int ManualRefund { get; set; }
    [Cell("G3")]
    public int AutoRefund { get; set; }
    [Cell("H3")]
    public double AutoRate
    {
        get
        {
            var total = ManualCalculate + AutoCalculate + ManualRefund + AutoRefund;
            var autoTotal = AutoCalculate + AutoRefund;
            return total == 0 ? 0 : (double)autoTotal / total;
        }
    }
}


public class SettleDurationCriteria : CriteriaBase
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public override string GetSettingKey() => "NAP_Set_SettleDurationReport";

    public override bool IsValid() => true;
}


public class AutoPayoutRateCriteria : CriteriaBase
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public override string GetSettingKey() => "NAP_Settlement_AutoSettle_Statistics";

    public override bool IsValid() => true;
}