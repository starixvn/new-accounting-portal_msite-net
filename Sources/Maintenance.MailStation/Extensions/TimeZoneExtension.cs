namespace Maintenance.MailStation.Extensions;

using System;
using Constants;

public static class TimeZoneExtension
{
    public static string GetTimeZoneString()
    {
        TimeZoneInfo.ClearCachedData();
        var standardName = TimeZoneInfo.Local.StandardName;
        var timezone = TimeZoneConstants.Timezones.Find(timeZone => timeZone.StandardName.Equals(standardName));
        return timezone?.TimeZoneString;
    }
}