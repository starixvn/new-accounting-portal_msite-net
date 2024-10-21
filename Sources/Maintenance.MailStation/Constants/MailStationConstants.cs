namespace Maintenance.MailStation.Constants;

using System.Collections.Generic;
using System.Globalization;
using Models;

public static class MailStationConstants
{
    public static List<SelectListItemExtra> Active { get; } =
    [
        new() { Text = "Active", Value = "1", Selected = false },
        new() { Text = "Inactive", Value = "0", Selected = false }
    ];

    public static List<SelectListItemExtra> TriggerTypes { get; } =
    [
        new() { Selected = false, Text = "Function", Value = "0" },
        new() { Selected = false, Text = "Schedule", Value = "1" }
    ];

    public static List<SelectListItemExtra> AllItem { get; } = [new() { Text = "All", Value = "-1" }];

    public static List<SelectListItemExtra> TaskStatus { get; } =
    [
        new() { Text = "Waiting".ToUpper(CultureInfo.InvariantCulture), Value = "0" },
        new() { Text = "Running".ToUpper(CultureInfo.InvariantCulture), Value = "1" },
        new() { Text = "Data not ready".ToUpper(CultureInfo.InvariantCulture), Value = "2" },
        new() { Text = "Failed".ToUpper(CultureInfo.InvariantCulture), Value = "3" },
        new() { Text = "Sent".ToUpper(CultureInfo.InvariantCulture), Value = "4" },
        new() { Text = "Aborted".ToUpper(CultureInfo.InvariantCulture), Value = "6" },
        new() { Text = "Succeeded".ToUpper(CultureInfo.InvariantCulture), Value = "7" },
        new() { Text = "Missing".ToUpper(CultureInfo.InvariantCulture), Value = "8" }
    ];
}