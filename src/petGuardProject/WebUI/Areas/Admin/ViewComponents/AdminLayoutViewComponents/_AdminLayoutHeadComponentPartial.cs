using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.ViewComponents.AdminLayoutViewComponents;

public class _AdminLayoutHeadComponentPartial: ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
