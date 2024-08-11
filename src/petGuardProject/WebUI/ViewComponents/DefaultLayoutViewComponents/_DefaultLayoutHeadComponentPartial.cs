using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents.DefaultLayoutViewComponents;

public class _DefaultLayoutHeadComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
