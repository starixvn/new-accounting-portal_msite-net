namespace Maintenance.MailStation.Constants;

using Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

public static class TimeZoneConstants
{
    [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals", Justification = "Reviewed. Suppression is OK here.")]
    static TimeZoneConstants()
    {
        Timezones = new List<CustomTimeZone>();
        Timezones.Add(new CustomTimeZone { StandardName = "Morocco Standard Time", DisplayName = "(GMT) Casablanca", TimeZoneString = "+00:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "GMT Standard Time", DisplayName = "(GMT) Greenwich Mean Time : Dublin, Edinburgh, Lisbon, London", TimeZoneString = "+00:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Greenwich Standard Time", DisplayName = "(GMT) Monrovia, Reykjavik", TimeZoneString = "+00:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "W. Europe Standard Time", DisplayName = "(GMT+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna", TimeZoneString = "+01:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Central Europe Standard Time", DisplayName = "(GMT+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague", TimeZoneString = "+01:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Romance Standard Time", DisplayName = "(GMT+01:00) Brussels, Copenhagen, Madrid, Paris", TimeZoneString = "+01:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Central European Standard Time", DisplayName = "(GMT+01:00) Sarajevo, Skopje, Warsaw, Zagreb", TimeZoneString = "+01:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "W. Central Africa Standard Time", DisplayName = "(GMT+01:00) West Central Africa", TimeZoneString = "+01:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Jordan Standard Time", DisplayName = "(GMT+02:00) Amman", TimeZoneString = "+02:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "GTB Standard Time", DisplayName = "(GMT+02:00) Athens, Bucharest, Istanbul", TimeZoneString = "+02:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Middle East Standard Time", DisplayName = "(GMT+02:00) Beirut", TimeZoneString = "+02:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Egypt Standard Time", DisplayName = "(GMT+02:00) Cairo", TimeZoneString = "+02:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "South Africa Standard Time", DisplayName = "(GMT+02:00) Harare, Pretoria", TimeZoneString = "+02:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "FLE Standard Time", DisplayName = "(GMT+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius", TimeZoneString = "+02:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Israel Standard Time", DisplayName = "(GMT+02:00) Jerusalem", TimeZoneString = "+02:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "E. Europe Standard Time", DisplayName = "(GMT+02:00) Minsk", TimeZoneString = "+02:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Namibia Standard Time", DisplayName = "(GMT+02:00) Windhoek", TimeZoneString = "+02:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Arabic Standard Time", DisplayName = "(GMT+03:00) Baghdad", TimeZoneString = "+03:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Arab Standard Time", DisplayName = "(GMT+03:00) Kuwait, Riyadh", TimeZoneString = "+03:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Russian Standard Time", DisplayName = "(GMT+03:00) Moscow, St. Petersburg, Volgograd", TimeZoneString = "+03:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "E. Africa Standard Time", DisplayName = "(GMT+03:00) Nairobi", TimeZoneString = "+03:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Georgian Standard Time", DisplayName = "(GMT+03:00) Tbilisi", TimeZoneString = "+03:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Iran Standard Time", DisplayName = "(GMT+03:30) Tehran", TimeZoneString = "+03:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Arabian Standard Time", DisplayName = "(GMT+04:00) Abu Dhabi, Muscat", TimeZoneString = "+04:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Azerbaijan Standard Time", DisplayName = "(GMT+04:00) Baku", TimeZoneString = "+04:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Mauritius Standard Time", DisplayName = "(GMT+04:00) Port Louis", TimeZoneString = "+04:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Caucasus Standard Time", DisplayName = "(GMT+04:00) Yerevan", TimeZoneString = "+04:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Afghanistan Standard Time", DisplayName = "(GMT+04:30) Kabul", TimeZoneString = "+04:30" });
        Timezones.Add(new CustomTimeZone { StandardName = "Ekaterinburg Standard Time", DisplayName = "(GMT+05:00) Ekaterinburg", TimeZoneString = "+05:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Pakistan Standard Time", DisplayName = "(GMT+05:00) Islamabad, Karachi", TimeZoneString = "+05:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "West Asia Standard Time", DisplayName = "(GMT+05:00) Tashkent", TimeZoneString = "+05:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "India Standard Time", DisplayName = "(GMT+05:30) Chennai, Kolkata, Mumbai, New Delhi", TimeZoneString = "+05:30" });
        Timezones.Add(new CustomTimeZone { StandardName = "Sri Lanka Standard Time", DisplayName = "(GMT+05:30) Sri Jayawardenepura", TimeZoneString = "+05:30" });
        Timezones.Add(new CustomTimeZone { StandardName = "Nepal Standard Time", DisplayName = "(GMT+05:45) Kathmandu", TimeZoneString = "+05:45" });
        Timezones.Add(new CustomTimeZone { StandardName = "N. Central Asia Standard Time", DisplayName = "(GMT+06:00) Almaty, Novosibirsk", TimeZoneString = "+06:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Central Asia Standard Time", DisplayName = "(GMT+06:00) Astana, Dhaka", TimeZoneString = "+06:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Myanmar Standard Time", DisplayName = "(GMT+06:30) Yangon (Rangoon)", TimeZoneString = "+06:30" });
        Timezones.Add(new CustomTimeZone { StandardName = "SE Asia Standard Time", DisplayName = "(GMT+07:00) Bangkok, Hanoi, Jakarta", TimeZoneString = "+07:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "North Asia Standard Time", DisplayName = "(GMT+07:00) Krasnoyarsk", TimeZoneString = "+07:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "China Standard Time", DisplayName = "(GMT+08:00) Beijing, Chongqing, Hong Kong, Urumqi", TimeZoneString = "+08:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "North Asia East Standard Time", DisplayName = "(GMT+08:00) Irkutsk, Ulaan Bataar", TimeZoneString = "+08:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Singapore Standard Time", DisplayName = "(GMT+08:00) Kuala Lumpur, Singapore", TimeZoneString = "+08:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "W. Australia Standard Time", DisplayName = "(GMT+08:00) Perth", TimeZoneString = "+08:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Taipei Standard Time", DisplayName = "(GMT+08:00) Taipei", TimeZoneString = "+08:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Tokyo Standard Time", DisplayName = "(GMT+09:00) Osaka, Sapporo, Tokyo", TimeZoneString = "+09:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Korea Standard Time", DisplayName = "(GMT+09:00) Seoul", TimeZoneString = "+09:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Yakutsk Standard Time", DisplayName = "(GMT+09:00) Yakutsk", TimeZoneString = "+09:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Cen. Australia Standard Time", DisplayName = "(GMT+09:30) Adelaide", TimeZoneString = "+09:30" });
        Timezones.Add(new CustomTimeZone { StandardName = "AUS Central Standard Time", DisplayName = "(GMT+09:30) Darwin", TimeZoneString = "+09:30" });
        Timezones.Add(new CustomTimeZone { StandardName = "E. Australia Standard Time", DisplayName = "(GMT+10:00) Brisbane", TimeZoneString = "+10:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "AUS Eastern Standard Time", DisplayName = "(GMT+10:00) Canberra, Melbourne, Sydney", TimeZoneString = "+10:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "West Pacific Standard Time", DisplayName = "(GMT+10:00) Guam, Port Moresby", TimeZoneString = "+10:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Tasmania Standard Time", DisplayName = "(GMT+10:00) Hobart", TimeZoneString = "+10:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Vladivostok Standard Time", DisplayName = "(GMT+10:00) Vladivostok", TimeZoneString = "+10:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Central Pacific Standard Time", DisplayName = "(GMT+11:00) Magadan, Solomon Is., New Caledonia", TimeZoneString = "+11:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "New Zealand Standard Time", DisplayName = "(GMT+12:00) Auckland, Wellington", TimeZoneString = "+12:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Fiji Standard Time", DisplayName = "(GMT+12:00) Fiji, Kamchatka, Marshall Is.", TimeZoneString = "+12:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Tonga Standard Time", DisplayName = "(GMT+13:00) Nuku'alofa", TimeZoneString = "+13:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Azores Standard Time", DisplayName = "(GMT-01:00) Azores", TimeZoneString = "-01:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Cape Verde Standard Time", DisplayName = "(GMT-01:00) Cape Verde Is.", TimeZoneString = "-01:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Mid-Atlantic Standard Time", DisplayName = "(GMT-02:00) Mid-Atlantic", TimeZoneString = "-02:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "E. South America Standard Time", DisplayName = "(GMT-03:00) Brasilia", TimeZoneString = "-03:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Argentina Standard Time", DisplayName = "(GMT-03:00) Buenos Aires", TimeZoneString = "-03:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "SA Eastern Standard Time", DisplayName = "(GMT-03:00) Georgetown", TimeZoneString = "-03:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Greenland Standard Time", DisplayName = "(GMT-03:00) Greenland", TimeZoneString = "-03:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Montevideo Standard Time", DisplayName = "(GMT-03:00) Montevideo", TimeZoneString = "-03:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Newfoundland Standard Time", DisplayName = "(GMT-03:30) Newfoundland", TimeZoneString = "-03:30" });
        Timezones.Add(new CustomTimeZone { StandardName = "Atlantic Standard Time", DisplayName = "(GMT-04:00) Atlantic Time (Canada)", TimeZoneString = "-03:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "SA Western Standard Time", DisplayName = "(GMT-04:00) La Paz", TimeZoneString = "-04:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Central Brazilian Standard Time", DisplayName = "(GMT-04:00) Manaus", TimeZoneString = "-04:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Pacific SA Standard Time", DisplayName = "(GMT-04:00) Santiago", TimeZoneString = "-04:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Venezuela Standard Time", DisplayName = "(GMT-04:30) Caracas", TimeZoneString = "-04:30" });
        Timezones.Add(new CustomTimeZone { StandardName = "SA Pacific Standard Time", DisplayName = "(GMT-05:00) Bogota, Lima, Quito, Rio Branco", TimeZoneString = "-05:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Eastern Standard Time", DisplayName = "(GMT-05:00) Eastern Time (US & Canada)", TimeZoneString = "-05:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "US Eastern Standard Time", DisplayName = "(GMT-05:00) Indiana (East)", TimeZoneString = "-05:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Central America Standard Time", DisplayName = "(GMT-06:00) Central America", TimeZoneString = "-06:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Central Standard Time", DisplayName = "(GMT-06:00) Central Time (US & Canada)", TimeZoneString = "-06:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Central Standard Time (Mexico)", DisplayName = "(GMT-06:00) Guadalajara, Mexico City, Monterrey", TimeZoneString = "-06:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Canada Central Standard Time", DisplayName = "(GMT-06:00) Saskatchewan", TimeZoneString = "-06:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "US Mountain Standard Time", DisplayName = "(GMT-07:00) Arizona", TimeZoneString = "-07:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Mountain Standard Time (Mexico)", DisplayName = "(GMT-07:00) Chihuahua, La Paz, Mazatlan", TimeZoneString = "-07:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Mountain Standard Time", DisplayName = "(GMT-07:00) Mountain Time (US & Canada)", TimeZoneString = "-07:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Pacific Standard Time", DisplayName = "(GMT-08:00) Pacific Time (US & Canada)", TimeZoneString = "-08:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Pacific Standard Time (Mexico)", DisplayName = "(GMT-08:00) Tijuana, Baja California", TimeZoneString = "-08:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Alaskan Standard Time", DisplayName = "(GMT-09:00) Alaska", TimeZoneString = "-09:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Hawaiian Standard Time", DisplayName = "(GMT-10:00) Hawaii", TimeZoneString = "-10:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Samoa Standard Time", DisplayName = "(GMT-11:00) Midway Island, Samoa", TimeZoneString = "-11:00" });
        Timezones.Add(new CustomTimeZone { StandardName = "Dateline Standard Time", DisplayName = "(GMT-12:00) International Date Line West", TimeZoneString = "-12:00" });
    }

    public static List<CustomTimeZone> Timezones { get; set; }
}