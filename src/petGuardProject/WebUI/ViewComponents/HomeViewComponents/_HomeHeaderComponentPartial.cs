using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents.HomeViewComponents;

public class _HomeHeaderComponentPartial: ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
