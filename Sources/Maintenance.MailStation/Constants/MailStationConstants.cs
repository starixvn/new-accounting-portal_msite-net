namespace Maintenance.MailStation.Constants;

using System.Collections.Generic;
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
        new() { Text = "Waiting".ToUpper(), Value = "0" },
        new() { Text = "Running".ToUpper(), Value = "1" },
        new() { Text = "Data not ready".ToUpper(), Value = "2" },
        new() { Text = "Failed".ToUpper(), Value = "3" },
        new() { Text = "Sent".ToUpper(), Value = "4" },
        new() { Text = "Aborted".ToUpper(), Value = "6" },
        new() { Text = "Succeeded".ToUpper(), Value = "7" },
        new() { Text = "Missing".ToUpper(), Value = "8" }
    ];
}