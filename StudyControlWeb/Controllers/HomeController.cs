using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudyControlWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Меню";
            if (User.IsInRole("Faculty"))       return RedirectToAction("Index", "Faculty");
            if (User.IsInRole("Department"))    return RedirectToAction("Index", "Department");
            if (User.IsInRole("Teacher"))       return RedirectToAction("Index", "Teacher");
            if (User.IsInRole("Student"))       return RedirectToAction("Index", "Student");
            return View();
        }
    }
}
