namespace Maintenance.MailStation.Models;

using Microsoft.AspNetCore.Mvc.Rendering;

public class SelectListItemExtra : SelectListItem
{
    public bool active
    {
        get;
        set;
    }
}