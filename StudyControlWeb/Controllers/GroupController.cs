using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyControlWeb.Data;
using StudyControlWeb.Data.Repositories;
using StudyControlWeb.Models.DBO;
using StudyControlWeb.ViewModels;

namespace StudyControlWeb.Controllers
{
    [Authorize(Roles = "Admin, Faculty, Department")]
    public class GroupController : Controller
    {
        private UniversityRepository db;
        public GroupController(DataContext context)
        {
            db = new UniversityRepository(context);
        }
        public IActionResult Groups()
        {
            if (User.IsInRole("Admin"))
            {
                var model = db.Groups.GetAll().
                    Select(g => new GroupViewModel()
                    {
                        Id = g.Id,
                        Code = g.Code,
                        Year = DateTime.Now.Year - g.StartDate.Year,
                        DepartmentTitle = g.Department.Title,
                        AreaTitle = g.Area.Title,
                        Profile = g.Area.Profile,
                        StartYear = g.StartDate.Year
                    });
                return View(model);
            }
            if (User.IsInRole("Faculty"))
            {
                var model = db.Groups.GetAll().
                    Where(g => g.FacultyId.ToString() == User.Identity.Name).
                    Select(g => new GroupViewModel()
                    {
                        Id = g.Id,
                        Code = g.Code,
                        Year = DateTime.Now.Year - g.StartDate.Year,
                        DepartmentTitle = g.Department.Title,
                        AreaTitle = g.Area.Title,
                        Profile = g.Area.Profile,
                        StartYear = g.StartDate.Year
                    });
                return View(model);
            }
            if (User.IsInRole("Department"))
            {
                var model = db.Groups.GetAll().
                    Where(g => g.DepartmentId.ToString() == User.Identity.Name).
                    Select(g => new GroupViewModel()
                    {
                        Id = g.Id,
                        Code = g.Code,
                        Year = DateTime.Now.Year - g.StartDate.Year,
                        DepartmentTitle = g.Department.Title,
                        AreaTitle = g.Area.Title,
                        Profile = g.Area.Profile,
                        StartYear = g.StartDate.Year
                    });
                return View(model);
            }
            return View();
        }
        public IActionResult CreateGroup()
        {
            var model = new GroupViewModel() { StartYear = DateTime.Now.Year };

            ViewBag.DepartmentTitles = DepartmentTitles();
            ViewBag.FacultyTitles = FacultyTitles();
            ViewBag.Areas = Areas();

            return View(model);
        }
        public IActionResult ViewGroup(int? id)
        {
            var gro = db.Groups.Get(id.ToString());
            if (gro != null)
            {
                var model = new GroupViewModel()
                {
                    Code = gro.Code,
                    Students = db.Students.GetAll().Where(s => s.GroupId == gro.Id).ToList(),
                };
                return View(model);
            }
            return View();
        }
        [HttpPost]
        public IActionResult CreateGroup(GroupViewModel model)
        {
            var group = new Group()
            {
                Code = model.Code,
                DepartmentId = db.Departments.GetAll().FirstOrDefault(d => d.Title == model.AreaTitle.Split("|")[3]).Id,
                FacultyId = db.Faculties.GetAll().FirstOrDefault(f => f.Title == model.FacultyTitle).Id,
                AreaId = db.Areas.GetAll().FirstOrDefault(a => a.Code == model.AreaTitle.Split("|")[0] &&
                                                                a.Title == model.AreaTitle.Split("|")[1] &&
                                                                a.Profile == model.AreaTitle.Split("|")[2]).Id,
                StartDate = new DateTime(model.StartYear, 9, 1)
            };

            db.Groups.Add(group);
            return RedirectToAction("Groups");
        }
        public IActionResult EditGroup(int? id)
        {
            if (id != null)
            {
                Group? gro = db.Groups.Get(id.ToString());
                if (gro != null)
                {
                    var model = new GroupViewModel()
                    {
                        Id = gro.Id,
                        Code = gro.Code,
                        DepartmentTitle = gro.Department.Title,
                        AreaTitle = gro.Area.Title,
                        StartYear = gro.StartDate.Year
                    };

                    ViewBag.DepartmentTitles = DepartmentTitles();
                    ViewBag.FacultyTitles = FacultyTitles();
                    ViewBag.Areas = Areas();

                    return View(model);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult EditGroup(GroupViewModel model)
        {
            var group = new Group()
            {
                Id = model.Id,
                Code = model.Code,
                DepartmentId = db.Departments.GetAll().FirstOrDefault(d => d.Title == model.AreaTitle.Split("|")[3]).Id,
                FacultyId = db.Faculties.GetAll().FirstOrDefault(f => f.Title == model.FacultyTitle).Id,
                AreaId = db.Areas.GetAll().FirstOrDefault(a =>  a.Code == model.AreaTitle.Split("|")[0] &&
                                                                a.Title == model.AreaTitle.Split("|")[1] &&
                                                                a.Profile == model.AreaTitle.Split("|")[2]).Id,
                StartDate = new DateTime(model.StartYear, 9, 1)
            };

            db.Groups.Update(group);
            return RedirectToAction("Groups");
        }
        [HttpPost]
        public IActionResult DeleteGroup(int? id)
        {
            if (id != null)
            {
                db.Groups.Delete(id.ToString());
                return RedirectToAction("Groups");
            }
            return NotFound();
        }
        private IEnumerable<SelectListItem> DepartmentTitles()
        {
            IEnumerable<string> titles = new List<string>();
            if (User.IsInRole("Admin"))
            {
                titles = db.Departments.GetAll().Select(d => d.Title);
            }
            if (User.IsInRole("Faculty"))
            {
                titles = db.Departments.GetAll().Select(d => d.Title);
            }
            if (User.IsInRole("Department"))
            {
                titles = new List<string>() { db.Departments.Get(User.Identity.Name).Title };
            }
            return titles.Select(t => new SelectListItem { Text = t, Value = t });
        }
        private IEnumerable<SelectListItem> FacultyTitles()
        {
            IEnumerable<string> titles = new List<string>();
            if (User.IsInRole("Admin"))
            {
                titles = db.Faculties.GetAll().Select(d => d.Title);
            }
            if (User.IsInRole("Faculty"))
            {
                titles = db.Faculties.GetAll().Where(f => f.Id.ToString() == User.Identity.Name).Select(d => d.Title);
            }
            if (User.IsInRole("Department"))
            {
                titles = db.Faculties.GetAll().Select(d => d.Title);
            }
            return titles.Select(t => new SelectListItem { Text = t, Value = t });
        }
        private IEnumerable<SelectListItem> Areas()
        {
            IEnumerable<string> titles = new List<string>();
            if (User.IsInRole("Admin"))
            {
                titles = db.Areas.GetAll().Select(a => $"{a.Code}|{a.Title}|{a.Profile}|{a.Department.Title}");
            }
            if (User.IsInRole("Faculty"))
            {
                titles = db.Areas.GetAll().Select(a => $"{a.Code}|{a.Title}|{a.Profile}|{a.Department.Title}");
            }
            if (User.IsInRole("Department"))
            {
                titles = db.Areas.GetAll().Where(a => a.DepartmentId.ToString() == User.Identity.Name).Select(a => $"{a.Code}|{a.Title}|{a.Profile}|{a.Department.Title}");
            }
            return titles.OrderBy(t => t).Select(t => new SelectListItem { Text = t, Value = t });
        }
    }
}
