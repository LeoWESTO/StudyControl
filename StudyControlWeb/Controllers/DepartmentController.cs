using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyControlWeb.Data;
using StudyControlWeb.Data.Repositories;
using StudyControlWeb.Models.DBO;
using StudyControlWeb.ViewModels;

namespace StudyControlWeb.Controllers
{
    [Authorize(Roles = "Admin, Faculty")]
    public class DepartmentController : Controller
    {
        private UniversityRepository db;
        public DepartmentController(DataContext context)
        {
            db = new UniversityRepository(context);
        }
        public IActionResult Departments()
        {
            if (User.IsInRole("Admin"))
            {
                var model = db.Departments.GetAll().
                    Select(d => new DepartmentViewModel()
                    {
                        Id = d.Id,
                        Title = d.Title,
                        FacultyTitle = d.Faculty.Title,
                        Password = d.Password,
                    });
                return View(model);
            }
            if (User.IsInRole("Faculty"))
            {
                var model = db.Departments.GetAll().
                    Where(d => d.FacultyId.ToString() == User.Identity.Name)
                    .Select(d => new DepartmentViewModel()
                    {
                        Id = d.Id,
                        Title = d.Title,
                        FacultyTitle = d.Faculty.Title,
                        Password = d.Password,
                    });
                return View(model);
            }
            return View();
        }
        public IActionResult CreateDepartment()
        {
            var list = db.Faculties.GetAll().Select(f => f.Title);
            ViewBag.Titles = list.Select(l => new SelectListItem { Text = l, Value = l });
            return View();
        }
        [HttpPost]
        public IActionResult CreateDepartment(DepartmentViewModel model)
        {
            var department = new Department()
            {
                Title = model.Title,
                FacultyId = db.Faculties.GetAll().FirstOrDefault(f => f.Title == model.FacultyTitle).Id,
                Password = model.Password,
            };

            db.Departments.Add(department);
            return RedirectToAction("Departments");
        }
        public IActionResult ViewAreas(int? id)
        {
            if (id != null)
            {
                var areas = db.Departments.Get(id.ToString()).Areas;
                if (areas != null)
                {
                    return View(areas);
                }
            }
            return NotFound();
        }
        public IActionResult EditDepartment(int? id)
        {
            if (id != null)
            {
                Department? dep = db.Departments.Get(id.ToString());
                if (dep != null)
                {
                    DepartmentViewModel model = new DepartmentViewModel()
                    {
                        Id = dep.Id,
                        Title = dep.Title,
                        FacultyTitle = dep.Faculty.Title,
                        Password = dep.Password,
                    };
                    var list = db.Faculties.GetAll().Select(f => f.Title);
                    ViewBag.Titles = list.Select(l => new SelectListItem { Text = l, Value = l });
                    return View(model);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult EditDepartment(DepartmentViewModel model)
        {
            var department = new Department()
            {
                Id = model.Id,
                FacultyId = db.Faculties.GetAll().FirstOrDefault(f => f.Title == model.FacultyTitle).Id,
                Password = model.Password,
                Title = model.Title,
            };
            db.Departments.Update(department);
            return RedirectToAction("Departments");
        }
        [HttpPost]
        public IActionResult DeleteDepartment(int? id)
        {
            if (id != null)
            {
                db.Departments.Delete(id.ToString());
                return RedirectToAction("Departments");
            }
            return NotFound();
        }
    }
}
