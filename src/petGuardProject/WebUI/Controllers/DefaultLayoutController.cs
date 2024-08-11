using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class DefaultLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
