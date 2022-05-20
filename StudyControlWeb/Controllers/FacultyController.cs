using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudyControlWeb.Controllers
{
    [Authorize]
    public class FacultyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
