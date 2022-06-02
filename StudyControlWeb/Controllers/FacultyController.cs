using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyControlWeb.Data;
using StudyControlWeb.Data.Repositories;
using StudyControlWeb.Models.DBO;

namespace StudyControlWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FacultyController : Controller
    {
        private UniversityRepository db;
        public FacultyController(DataContext context)
        {
            db = new UniversityRepository(context);
        }
        public IActionResult Faculties()
        {
            return View(db.Faculties.GetAll());
        }
        public IActionResult CreateFaculty()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateFaculty(Faculty faculty)
        {
            db.Faculties.Add(faculty);
            return RedirectToAction("Faculties");
        }
        public IActionResult EditFaculty(int? id)
        {
            if (id != null)
            {
                Faculty? fac = db.Faculties.Get(id.ToString());
                if (fac != null) return View(fac);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult EditFaculty(Faculty faculty)
        {
            db.Faculties.Update(faculty);
            return RedirectToAction("Faculties");
        }
        [HttpPost]
        public IActionResult DeleteFaculty(int? id)
        {
            if (id != null)
            {
                db.Faculties.Delete(id.ToString());
                return RedirectToAction("Faculties");
            }
            return NotFound();
        }
    }
}
