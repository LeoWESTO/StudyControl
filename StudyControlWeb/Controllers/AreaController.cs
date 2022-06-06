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
                        Profile = a.Profile,
                        DepartmentTitle = a.Department.Title,
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
                    Profile = a.Profile,
                    DepartmentTitle = a.Department.Title,
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
                Degree = model.DegreeName switch
                {
                    "Бакалавриат" => Degree.Bachelor,
                    "Магистратура" => Degree.Master,
                    "Специалитет" => Degree.Specialist,
                    "Аспирантура" => Degree.Postgraduate,
                },
                DepartmentId = db.Departments.GetAll().FirstOrDefault(d => d.Title == model.DepartmentTitle).Id,
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
                        DepartmentTitle = area.Department.Title,
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
        public IActionResult DeleteArea(int? id)
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
    }
}
