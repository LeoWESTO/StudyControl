using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyControlWeb.Data;
using StudyControlWeb.Data.Repositories;
using StudyControlWeb.Models.DBO;
using StudyControlWeb.ViewModels;

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
            return View(db.Faculties.GetAll().Select(f => new FacultyViewModel()
            {
                Id = f.Id,
                Title = f.Title,
            }));
        }
        public IActionResult CreateFaculty()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateFaculty(FacultyViewModel model)
        {
            var faculty = new Faculty()
            {
                Id = model.Id,
                Title = model.Title,
                Password = model.Password,
            };
            db.Faculties.Add(faculty);
            return RedirectToAction("Faculties");
        }
        public IActionResult EditFaculty(int id)
        {
            Faculty? fac = db.Faculties.Get(id.ToString());
            if (fac != null) return View(new FacultyViewModel()
            {
                Id = fac.Id,
                Title = fac.Title,
            });
            return NotFound();
        }
        [HttpPost]
        public IActionResult EditFaculty(FacultyViewModel model)
        {
            var faculty = new Faculty()
            {
                Id = model.Id,
                Title = model.Title,
                Password = model.Password,
            };
            db.Faculties.Update(faculty);
            return RedirectToAction("Faculties");
        }
        [HttpPost]
        public IActionResult DeleteFaculty(int id)
        {
            db.Faculties.Delete(id.ToString());
            return RedirectToAction("Faculties");
        }
    }
}
