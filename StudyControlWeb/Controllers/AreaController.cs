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
        public IActionResult Areas(int? id)
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
                return View(model);
            }
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
            return View(model);
        }
        public IActionResult CreateArea()
        {
            ViewBag.Titles = Titles();
            ViewBag.Degrees = Degrees();
            ViewBag.Forms = Forms();
            return View();
        }
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
