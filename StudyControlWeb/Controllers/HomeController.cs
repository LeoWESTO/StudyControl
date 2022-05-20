using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudyControlWeb.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            ViewBag.Title = "Меню";
            return View();
        }
        [Authorize]
        public IActionResult Points()
        {
            ViewBag.Title = "Успеваемость";
            return View();
        }
        [Authorize]
        public IActionResult Timetable()
        {
            ViewBag.Title = "Расписание";
            return View();
        }
    }
}
