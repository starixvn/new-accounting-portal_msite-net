namespace Maintenance.MailStation.Extensions;

using Constants;
using System;
using System.Linq;

public static class TimeZoneExtension
{
    public static string GetTimeZoneString()
    {
        TimeZoneInfo.ClearCachedData();
        var standardName = TimeZone.CurrentTimeZone.StandardName;
        var timezone = TimeZoneConstants.Timezones.FirstOrDefault(timeZone => timeZone.StandardName.Equals(standardName));
        return timezone?.TimeZoneString;
    }
}