namespace Maintenance.MailStation.Constants;

using Models;
using System.Collections.Generic;

public static class MailStationConstants
{
    public static List<SelectListItemExtra> Active { get; } = new()
    {
        new SelectListItemExtra { Text = "Active", Value = "1", Selected = false },
        new SelectListItemExtra { Text = "Inactive", Value = "0", Selected = false }
    };

    public static List<SelectListItemExtra> TriggerTypes { get; } = new()
    {
        new SelectListItemExtra { Selected = false, Text = "Function", Value = "0" },
        new SelectListItemExtra { Selected = false, Text = "Schedule", Value = "1" }
    };

    public static List<SelectListItemExtra> AllItem { get; } = new()
    {
        new SelectListItemExtra { Text = "All", Value = "-1" }
    };

    public static List<SelectListItemExtra> TaskStatus { get; } = new()
    {
        new SelectListItemExtra { Text = "Waiting".ToUpper(), Value = "0" },
        new SelectListItemExtra { Text = "Running".ToUpper(), Value = "1" },
        new SelectListItemExtra { Text = "Data not ready".ToUpper(), Value = "2" },
        new SelectListItemExtra { Text = "Failed".ToUpper(), Value = "3" },
        new SelectListItemExtra { Text = "Sent".ToUpper(), Value = "4" },
        new SelectListItemExtra { Text = "Aborted".ToUpper(), Value = "6" },
        new SelectListItemExtra { Text = "Succeeded".ToUpper(), Value = "7" },
        new SelectListItemExtra { Text = "Missing".ToUpper(), Value = "8" }
    };
}