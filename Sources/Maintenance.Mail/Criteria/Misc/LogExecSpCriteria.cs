namespace Maintenance.Mail.Criteria.Misc;

using System;

public class LogExecSpCriteria
{
    public int CurrentUserId { get; set; }

    public DateTime LogDate { get; set; }

    public string LoginIp { get; set; }
}