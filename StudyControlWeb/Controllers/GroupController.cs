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
                        DepartmentTitle = g.Department.Title,
                        AreaTitle = g.Area.Title,
                        Profile = g.Profile,
                        StartYear = g.StartDate.Year
                    });
                return View(model);
            }
            if (User.IsInRole("Faculty"))
            {
                var model = db.Groups.GetAll().
                    Where(g => g.Department.FacultyId.ToString() == User.Identity.Name).
                    Select(g => new GroupViewModel()
                    {
                        Id = g.Id,
                        Code = g.Code,
                        DepartmentTitle = g.Department.Title,
                        AreaTitle = g.Area.Title,
                        Profile = g.Profile,
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
                        DepartmentTitle = g.Department.Title,
                        AreaTitle = g.Area.Title,
                        Profile = g.Profile,
                        StartYear = g.StartDate.Year
                    });
                return View(model);
            }
            return View();
        }
        public IActionResult CreateGroup()
        {
            var model = new GroupViewModel() { StartYear = DateTime.Now.Year };

            ViewBag.Titles = Titles();
            var areas = db.Areas.GetAll().Where(a => a.IsActive).Select(a => a.Title);
            ViewBag.Areas = areas.Select(a => new SelectListItem { Text = a, Value = a });

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
                DepartmentId = db.Departments.GetAll().FirstOrDefault(d => d.Title == model.DepartmentTitle).Id,
                AreaId = db.Areas.GetAll().FirstOrDefault(a => a.Title == model.AreaTitle).Id,
                Profile = model.Profile,
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
                        Profile = gro.Profile,
                        StartYear = gro.StartDate.Year
                    };

                    
                    ViewBag.Titles = Titles();
                    var areas = db.Areas.GetAll().Where(a => a.IsActive).Select(a => a.Title);
                    ViewBag.Areas = areas.Select(a => new SelectListItem { Text = a, Value = a });

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
                DepartmentId = db.Departments.GetAll().FirstOrDefault(d => d.Title == model.DepartmentTitle).Id,
                AreaId = db.Areas.GetAll().FirstOrDefault(a => a.Title == model.AreaTitle).Id,
                Profile = model.Profile,
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
