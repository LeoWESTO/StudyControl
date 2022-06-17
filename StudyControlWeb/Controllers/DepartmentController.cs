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
        public IActionResult Departments(string search, SortState sortState = SortState.FirstAsc, int page = 1)
        {
            IEnumerable<DepartmentViewModel> model = new List<DepartmentViewModel>();
            if (User.IsInRole("Admin"))
            {
                model = db.Departments.GetAll().
                    Select(d => new DepartmentViewModel()
                    {
                        Id = d.Id,
                        Title = d.Title,
                        FacultyTitle = d.Faculty.Title,
                        Password = d.Password,
                    });
            }
            if (User.IsInRole("Faculty"))
            {
                model = db.Departments.GetAll().
                    Where(d => d.FacultyId.ToString() == User.Identity.Name)
                    .Select(d => new DepartmentViewModel()
                    {
                        Id = d.Id,
                        Title = d.Title,
                        FacultyTitle = d.Faculty.Title,
                        Password = d.Password,
                    });
            }

            //фильтрация
            if (!String.IsNullOrEmpty(search))
            {
                model = model.Where(f => f.Title.ToUpper().Contains(search.ToUpper()));
                ViewBag.Search = search;
            }

            //сортировка
            model = sortState switch
            {
                SortState.FirstAsc => model.OrderBy(s => s.Title),
                SortState.FirstDesc => model.OrderByDescending(s => s.Title),
                SortState.SecondAsc => model.OrderBy(s => s.FacultyTitle),
                SortState.SecondDesc => model.OrderByDescending(s => s.FacultyTitle),
                _ => model.OrderBy(s => s.Title),
            };
            ViewBag.SortModel = new SortViewModel(sortState);

            //пагинация
            int count = model.Count();
            int pageSize = 10;
            model = model.Skip((page - 1) * pageSize).Take(pageSize);
            ViewBag.PageModel = new PageViewModel(count, page, pageSize);

            return View(model);
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
        public IActionResult EditDepartment(int? id)
        {
            if (id != null)
            {
                Department? dep = db.Departments.Get(id.ToString());
                if (dep != null)
                {
                    if (User.IsInRole("Faculty") && dep.FacultyId.ToString() != User.Identity.Name) return NotFound();

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
                if (User.IsInRole("Faculty") && db.Departments.Get(id.ToString()).FacultyId.ToString() != User.Identity.Name) return NotFound();

                db.Departments.Delete(id.ToString());
                return RedirectToAction("Departments");
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult CopyDepartment(int id)
        {
            var department = db.Departments.Get(id.ToString());
            if (department != null)
            {
                if (User.IsInRole("Faculty") && department.FacultyId.ToString() != User.Identity.Name) return NotFound();

                var copyDepartment = new Department()
                {
                    Title = department.Title,
                    FacultyId = department.FacultyId,
                    Password = department.Password,
                };

                db.Departments.Add(copyDepartment);
                return RedirectToAction("Departments");
            }
            return NotFound();
        }
    }
}
