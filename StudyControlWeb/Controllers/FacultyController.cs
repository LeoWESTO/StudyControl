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
        public IActionResult Faculties(string search, SortState sortState = SortState.FirstAsc, int page = 1)
        {
            var model = db.Faculties.GetAll().Select(f => new FacultyViewModel()
            {
                Id = f.Id,
                Title = f.Title,
            });

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
        [HttpPost]
        public IActionResult CopyFaculty(int id)
        {
            var faculty = db.Faculties.Get(id.ToString());
            if (faculty != null)
            {
                var copyFaculty = new Faculty()
                {
                    Title = faculty.Title,
                    Password = faculty.Password,
                };
                db.Faculties.Add(copyFaculty);
                return RedirectToAction("Faculties");
            }
            return NotFound();
        }
    }
}
