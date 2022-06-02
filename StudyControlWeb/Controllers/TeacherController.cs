using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyControlWeb.Data;
using StudyControlWeb.Data.Repositories;
using StudyControlWeb.Models.DBO;
using StudyControlWeb.ViewModels;

namespace StudyControlWeb.Controllers
{
    [Authorize(Roles = "Admin, Faculty, Department")]
    public class TeacherController : Controller
    {
        private UniversityRepository db;
        public TeacherController(DataContext context)
        {
            db = new UniversityRepository(context);
        }
        public IActionResult Teachers()
        {
            if (User.IsInRole("Admin"))
            {
                var model = db.Teachers.GetAll().
                    Select(s => new TeacherViewModel()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Surname = s.Surname,
                        Fathername = s.Fathername,
                        DepartmentTitle = s.Department.Title,
                        Password = s.Password
                    });
                return View(model);
            }
            if (User.IsInRole("Faculty"))
            {
                var model = db.Teachers.GetAll().
                    Where(t => t.Department.FacultyId.ToString() == User.Identity.Name).
                    Select(t => new TeacherViewModel()
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Surname = t.Surname,
                        Fathername = t.Fathername,
                        DepartmentTitle = t.Department.Title,
                        Password = t.Password
                    });
                return View(model);
            }
            if (User.IsInRole("Department"))
            {
                var model = db.Teachers.GetAll().
                    Where(t => t.DepartmentId.ToString() == User.Identity.Name).
                    Select(t => new TeacherViewModel()
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Surname = t.Surname,
                        Fathername = t.Fathername,
                        DepartmentTitle = t.Department.Title,
                        Password = t.Password
                    });
                return View(model);
            }
            return View();
        }
        public IActionResult CreateTeacher()
        {
            ViewBag.Titles = Titles();
            return View();
        }
        [HttpPost]
        public IActionResult CreateTeacher(TeacherViewModel model)
        {
            var teacher = new Teacher()
            {
                Id = model.Id,
                Name = model.Name,
                Surname = model.Surname,
                Fathername = model.Fathername,
                DepartmentId = db.Departments.GetAll().FirstOrDefault(d => d.Title == model.DepartmentTitle).Id,
                Password = model.Password,
            };

            db.Teachers.Add(teacher);
            return RedirectToAction("Teachers");
        }
        public IActionResult EditTeacher(int? id)
        {
            if (id != null)
            {
                Teacher? tea = db.Teachers.Get(id.ToString());
                if (tea != null)
                {
                    var model = new TeacherViewModel()
                    {
                        Id = tea.Id,
                        Name = tea.Name,
                        Surname = tea.Surname,
                        Fathername = tea.Fathername,
                        DepartmentTitle = tea.Department.Title,
                        Password = tea.Password
                    };
                    ViewBag.Titles = Titles();

                    return View(model);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult EditTeacher(TeacherViewModel model)
        {
            var teacher = new Teacher()
            {
                Id = model.Id,
                Name = model.Name,
                Surname = model.Surname,
                Fathername = model.Fathername,
                DepartmentId = db.Departments.GetAll().FirstOrDefault(d => d.Title == model.DepartmentTitle).Id,
                Password = model.Password,
            };

            db.Teachers.Update(teacher);
            return RedirectToAction("Teachers");
        }
        [HttpPost]
        public IActionResult DeleteTeacher(int? id)
        {
            if (id != null)
            {
                db.Teachers.Delete(id.ToString());
                return RedirectToAction("Teachers");
            }
            return NotFound();
        }

        private IEnumerable<SelectListItem> Titles()
        {
            IEnumerable<string> titles = new List<string>();
            if (User.IsInRole("Admin"))
            {
                titles = db.Departments.GetAll().Select(d => d.Title);
            }
            if (User.IsInRole("Faculty"))
            {
                titles = db.Departments.GetAll().Where(d => d.FacultyId.ToString() == User.Identity.Name).Select(d => d.Title);
            }
            if (User.IsInRole("Department"))
            {
                titles = new List<string>() { db.Departments.Get(User.Identity.Name).Title };
            }
            return titles.Select(t => new SelectListItem { Text = t, Value = t });
        }
    }
}
