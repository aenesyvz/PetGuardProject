using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents.HomeViewComponents;

public class _HomeFooterComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
