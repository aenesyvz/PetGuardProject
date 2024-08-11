using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents.HomeViewComponents;

public class _HomeNewsComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}