using Microsoft.AspNetCore.Mvc;

namespace StudyControlWeb.Controllers
{
    public class TimetableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
