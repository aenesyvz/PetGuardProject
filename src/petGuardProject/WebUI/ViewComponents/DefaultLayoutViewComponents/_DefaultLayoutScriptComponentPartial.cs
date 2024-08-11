using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents.DefaultLayoutViewComponents;

public class _DefaultLayoutScriptComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}