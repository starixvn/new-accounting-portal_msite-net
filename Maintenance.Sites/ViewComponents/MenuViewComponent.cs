using Microsoft.AspNetCore.Mvc;

namespace Maintenance.Sites.ViewComponents;

public class MenuViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("Default");
    }
}