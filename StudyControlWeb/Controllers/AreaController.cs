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
    public class AreaController : Controller
    {
        private UniversityRepository db;
        public AreaController(DataContext context)
        {
            db = new UniversityRepository(context);
        }
        public IActionResult Areas(int? id, string search, SortState sortState = SortState.FirstAsc, int page = 1)
        {
            IEnumerable<AreaViewModel> model;
            if(id == null)
            {
                model = db.Areas.GetAll().
                    Select(a => new AreaViewModel()
                    {
                        Id = a.Id,
                        Code = a.Code,
                        Title = a.Title,
                        Profile = string.Join(string.Empty, a.Profile.Split(' ', '-').Select(s => char.ToUpper(s[0]))),
                        Form = a.Form,
                        DepartmentTitle = a.Department.Title,
                        TermsCount = a.TermsCount,
                        DegreeName = a.Degree switch
                        {
                            Degree.Bachelor => "Бакалавриат",
                            Degree.Master => "Магистратура",
                            Degree.Specialist => "Специалитет",
                            Degree.Postgraduate => "Аспирантура",
                        }
                    });
            }
            else
            {
                model = db.Areas.GetAll().
                    Where(a => a.DepartmentId == id).
                    Select(a => new AreaViewModel()
                    {
                        Id = a.Id,
                        Code = a.Code,
                        Title = a.Title,
                        Profile = string.Join(string.Empty, a.Profile.Split(' ', '-').Select(s => char.ToUpper(s[0]))),
                        Form = a.Form,
                        DepartmentTitle = a.Department.Title,
                        TermsCount = a.TermsCount,
                        DegreeName = a.Degree switch
                        {
                            Degree.Bachelor => "Бакалавриат",
                            Degree.Master => "Магистратура",
                            Degree.Specialist => "Специалитет",
                            Degree.Postgraduate => "Аспирантура",
                        }
                    });
            }
            
            //фильтрация
            if (!String.IsNullOrEmpty(search))
            {
                model = model.Where(a => a.Title.ToUpper().Contains(search.ToUpper()));
                ViewBag.Search = search;
            }

            //сортировка
            model = sortState switch
            {
                SortState.FirstAsc => model.OrderBy(s => s.Code),
                SortState.FirstDesc => model.OrderByDescending(s => s.Code),
                SortState.SecondAsc => model.OrderBy(s => s.DegreeName),
                SortState.SecondDesc => model.OrderByDescending(s => s.DegreeName),
                SortState.ThirdAsc => model.OrderBy(s => s.Title),
                SortState.ThirdDesc => model.OrderByDescending(s => s.Title),
                SortState.FourthAsc => model.OrderBy(s => s.Form),
                SortState.FourthDesc => model.OrderByDescending(s => s.Form),
                SortState.FifthAsc => model.OrderBy(s => s.DepartmentTitle),
                SortState.FifthDesc => model.OrderByDescending(s => s.DepartmentTitle),
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
        [Authorize(Roles = "Admin")]
        public IActionResult CreateArea()
        {
            ViewBag.Titles = Titles();
            ViewBag.Degrees = Degrees();
            ViewBag.Forms = Forms();
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateArea(AreaViewModel model)
        {
            var area = new Area()
            {
                Id = model.Id,
                Code = model.Code,
                Title = model.Title,
                Profile = model.Profile,
                Form = model.Form,
                Degree = model.DegreeName switch
                {
                    "Бакалавриат" => Degree.Bachelor,
                    "Магистратура" => Degree.Master,
                    "Специалитет" => Degree.Specialist,
                    "Аспирантура" => Degree.Postgraduate,
                },
                DepartmentId = db.Departments.GetAll().FirstOrDefault(d => d.Title == model.DepartmentTitle).Id,
                TermsCount = model.TermsCount,
            };
            db.Areas.Add(area);
            return RedirectToAction("Areas");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult EditArea(int? id)
        {
            if (id != null)
            {
                Area? area = db.Areas.Get(id.ToString());
                if (area != null)
                {
                    var model = new AreaViewModel()
                    {
                        Id = area.Id,
                        Code = area.Code,
                        Title = area.Title,
                        Profile = area.Profile,
                        Form = area.Form,
                        DepartmentTitle = area.Department.Title,
                        TermsCount = area.TermsCount,
                        DegreeName = area.Degree switch
                        {
                            Degree.Bachelor => "Бакалавриат",
                            Degree.Master => "Магистратура",
                            Degree.Specialist => "Специалитет",
                            Degree.Postgraduate => "Аспирантура",
                        }
                    };

                    ViewBag.Titles = Titles();
                    ViewBag.Degrees = Degrees();
                    ViewBag.Forms = Forms();

                    return View(model);
                }
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditArea(AreaViewModel model)
        {
            var area = new Area()
            {
                Id = model.Id,
                Code = model.Code,
                Title = model.Title,
                Profile = model.Profile,
                Form = model.Form,
                TermsCount = model.TermsCount,
                Degree = model.DegreeName switch
                {
                    "Бакалавриат" => Degree.Bachelor,
                    "Магистратура" => Degree.Master,
                    "Специалитет" => Degree.Specialist,
                    "Аспирантура" => Degree.Postgraduate,
                },
                DepartmentId = db.Departments.GetAll().FirstOrDefault(d => d.Title == model.DepartmentTitle).Id,
            };

            db.Areas.Update(area);
            return RedirectToAction("Areas");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteArea(int id)
        {
            if (id != null)
            {
                db.Areas.Delete(id.ToString());
                return RedirectToAction("Areas");
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CopyArea(int id)
        {
            var area = db.Areas.Get(id.ToString());
            if (area != null)
            {
                var copyArea = new Area()
                {
                    Code = area.Code,
                    Title = area.Title,
                    Profile = area.Profile,
                    Form = area.Form,
                    Degree = area.Degree,
                    DepartmentId = area.DepartmentId,
                    TermsCount = area.TermsCount,
                };
                db.Areas.Add(copyArea);

                foreach (var s in db.Subjects.GetAll().Where(s => s.AreaId == id))
                {
                    var copySubject = new Subject()
                    {
                        AreaId = copyArea.Id,
                        Title = s.Title,
                        TeacherId = s.TeacherId,
                        TermNumber = s.TermNumber,
                        ControlTypes = s.ControlTypes,
                    };
                    db.Subjects.Add(copySubject);
                }

                return RedirectToAction("Areas");
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
        private IEnumerable<SelectListItem> Degrees()
        {
            IEnumerable<string> titles = new List<string>()
            {
                "Бакалавриат",
                "Магистратура",
                "Специалитет",
                "Аспирантура",
            };

            return titles.Select(t => new SelectListItem { Text = t, Value = t });
        }
        private IEnumerable<SelectListItem> Forms()
        {
            IEnumerable<string> titles = new List<string>()
            {
                "Очная",
                "Очно-заочная",
                "Заочная",
            };

            return titles.Select(t => new SelectListItem { Text = t, Value = t });
        }
    }
}
