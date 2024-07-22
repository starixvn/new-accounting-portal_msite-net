namespace Maintenance.MailStation.Criteria.MailReport;

using Fanex.Data;
using Fanex.Data.Repository;
using System;

public class MailReportCriteria : CriteriaBase
{
    public string MailName { get; set; }

    public int SiteId { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public int Status { get; set; }

    public int TriggerType { get; set; }

    [SpParam(IsSpecialType = true)]
    public byte[] TransformJson { get; set; }

    public override string GetSettingKey() => "MailStation_MailReport_GetMailTaskList";

    public override bool IsValid() => true;
}