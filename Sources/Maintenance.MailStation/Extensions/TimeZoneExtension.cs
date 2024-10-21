namespace Maintenance.MailStation.Extensions;

using System;
using System.Linq;
using Constants;

public static class TimeZoneExtension
{
    public static string GetTimeZoneString()
    {
        TimeZoneInfo.ClearCachedData();
        var standardName = TimeZoneInfo.Local.StandardName;
        var timezone = TimeZoneConstants.Timezones.FirstOrDefault(timeZone => timeZone.StandardName.Equals(standardName));
        return timezone?.TimeZoneString;
    }
}