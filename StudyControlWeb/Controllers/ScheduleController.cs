using Microsoft.AspNetCore.Mvc;

namespace StudyControlWeb.Controllers
{
    public class ScheduleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
