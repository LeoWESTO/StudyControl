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
        public IActionResult Groups(string search, SortState sortState = SortState.FirstAsc, int page = 1)
        {
            IEnumerable<GroupViewModel> model = new List<GroupViewModel>();
            if (User.IsInRole("Admin"))
            {
                model = db.Groups.GetAll().
                    Select(g => new GroupViewModel()
                    {
                        Id = g.Id,
                        Code = g.Code,
                        Year = DateTime.Now.Year - g.StartDate.Year,
                        DepartmentTitle = g.Department.Title,
                        AreaTitle = g.Area.Title,
                        Profile = string.Join(string.Empty, g.Area.Profile.Split(' ', '-').Select(s => char.ToUpper(s[0]))),
                        StartYear = g.StartDate.Year
                    });
            }
            if (User.IsInRole("Faculty"))
            {
                model = db.Groups.GetAll().
                    Where(g => g.FacultyId.ToString() == User.Identity.Name).
                    Select(g => new GroupViewModel()
                    {
                        Id = g.Id,
                        Code = g.Code,
                        Year = DateTime.Now.Year - g.StartDate.Year,
                        DepartmentTitle = g.Department.Title,
                        AreaTitle = g.Area.Title,
                        Profile = string.Join(string.Empty, g.Area.Profile.Split(' ', '-').Select(s => char.ToUpper(s[0]))),
                        StartYear = g.StartDate.Year
                    });
            }
            if (User.IsInRole("Department"))
            {
                model = db.Groups.GetAll().
                    Where(g => g.DepartmentId.ToString() == User.Identity.Name).
                    Select(g => new GroupViewModel()
                    {
                        Id = g.Id,
                        Code = g.Code,
                        Year = DateTime.Now.Year - g.StartDate.Year,
                        DepartmentTitle = g.Department.Title,
                        AreaTitle = g.Area.Title,
                        Profile = string.Join(string.Empty, g.Area.Profile.Split(' ', '-').Select(s => char.ToUpper(s[0]))),
                        StartYear = g.StartDate.Year
                    });
            }

            //фильтрация
            if (!String.IsNullOrEmpty(search))
            {
                model = model.Where(f => f.Code.ToUpper().Contains(search.ToUpper()));
                ViewBag.Search = search;
            }

            //сортировка
            model = sortState switch
            {
                SortState.FirstAsc => model.OrderBy(s => s.Code),
                SortState.FirstDesc => model.OrderByDescending(s => s.Code),
                SortState.SecondAsc => model.OrderBy(s => s.Year),
                SortState.SecondDesc => model.OrderByDescending(s => s.Year),
                SortState.ThirdAsc => model.OrderBy(s => s.DepartmentTitle),
                SortState.ThirdDesc => model.OrderByDescending(s => s.DepartmentTitle),
                SortState.FourthAsc => model.OrderBy(s => s.AreaTitle),
                SortState.FourthDesc => model.OrderByDescending(s => s.AreaTitle),
                SortState.FifthAsc => model.OrderBy(s => s.Profile),
                SortState.FifthDesc => model.OrderByDescending(s => s.Profile),
                _ => model.OrderBy(s => s.Code),
            };
            ViewBag.SortModel = new SortViewModel(sortState);

            //пагинация
            int count = model.Count();
            int pageSize = 10;
            model = model.Skip((page - 1) * pageSize).Take(pageSize);
            ViewBag.PageModel = new PageViewModel(count, page, pageSize);

            return View(model);
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
        [HttpPost]
        public IActionResult CopyGroup(int id)
        {
            var group = db.Groups.Get(id.ToString());
            if (group != null)
            {
                var copyGroup = new Group()
                {
                    Code = group.Code,
                    DepartmentId = group.DepartmentId,
                    FacultyId = group.FacultyId,
                    AreaId = group.AreaId,
                    StartDate = group.StartDate,
                };

                db.Groups.Add(copyGroup);
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
