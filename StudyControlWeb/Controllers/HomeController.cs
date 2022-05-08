using Microsoft.AspNetCore.Mvc;

namespace StudyControlWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Главное";
            return View();
        }
    }
}
