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
    public class SubjectController : Controller
    {
        private UniversityRepository db;
        public SubjectController(DataContext context)
        {
            db = new UniversityRepository(context);
        }
        public IActionResult Subjects(int id)
        {
            AreaViewModel model = new AreaViewModel();
            var area = db.Areas.Get(id.ToString());
            if(area != null)
            {
                model = new AreaViewModel()
                {
                    Id = id,
                    Title = area.Title,
                    Profile = area.Profile,
                    Code = area.Code,
                    DepartmentTitle = area.Department.Title,
                };
                var subjects = db.Subjects.GetAll().
                    Where(s => s.AreaId == id).
                    Select(s => new SubjectViewModel()
                    {
                        Id = s.Id,
                        Title = s.Title,
                        ControlType = s.ControlType,
                        TeacherFullName = $"{s.Teacher.Surname} {s.Teacher.Name} {s.Teacher.Fathername}",
                        TermNumber = s.TermNumber,
                    });

                model.Subjects = subjects.OrderBy(s => s.TermNumber).ToList();
            }
            return View(model);
        }
        public IActionResult CreateSubject(int areaId)
        {
            var titles = db.Teachers.GetAll().Select(t => $"{t.Surname} {t.Name} {t.Fathername}");
            var types = new List<string>() { "Зачет", "Диф.зачет", "Экзамен" };
            ViewBag.Teachers = titles.Select(t => new SelectListItem { Text = t, Value = t });
            ViewBag.ControlTypes = types.Select(t => new SelectListItem { Text = t, Value = t });
            ViewBag.Areas = new List<SelectListItem>() { new SelectListItem { Text = areaId.ToString(), Value = areaId.ToString() } };
            return View();
        }
        [HttpPost]
        public IActionResult CreateSubject(SubjectViewModel model, int id)
        {
            var subject = new Subject()
            {
                AreaId = id,
                ControlType = model.ControlType,
                Title = model.Title,
                TermNumber = model.TermNumber,
                TeacherId = db.Teachers.GetAll().FirstOrDefault(t =>    t.Surname == model.TeacherFullName.Split(" ")[0] &&
                                                                        t.Name == model.TeacherFullName.Split(" ")[1] &&
                                                                        t.Fathername == model.TeacherFullName.Split(" ")[2]).Id,
            };
            db.Subjects.Add(subject);
            return RedirectToAction("Subjects", new { id });
        }
        public IActionResult EditSubject(int id)
        {
            Subject? subject = db.Subjects.Get(id.ToString());
            if (subject != null)
            {
                var model = new SubjectViewModel()
                {
                    Id = id,
                    ControlType = subject.ControlType,
                    TermNumber = subject.TermNumber,
                    Title = subject.Title,
                    AreaId = subject.AreaId,
                    TeacherFullName = $"{subject.Teacher.Surname} {subject.Teacher.Name} {subject.Teacher.Fathername}",
                };
                var titles = db.Teachers.GetAll().Select(t => $"{t.Surname} {t.Name} {t.Fathername}");
                var types = new List<string>() { "Зачет", "Диф.зачет", "Экзамен" };
                ViewBag.Teachers = titles.Select(t => new SelectListItem { Text = t, Value = t });
                ViewBag.ControlTypes = types.Select(t => new SelectListItem { Text = t, Value = t });
                ViewBag.Areas = new List<SelectListItem>() { new SelectListItem { Text = subject.AreaId.ToString(), Value = subject.AreaId.ToString() } };
                return View(model);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult EditSubject(SubjectViewModel model)
        {
            var subject = new Subject()
            {
                Id = model.Id,
                AreaId = model.AreaId,
                ControlType = model.ControlType,
                Title = model.Title,
                TermNumber = model.TermNumber,
                TeacherId = db.Teachers.GetAll().FirstOrDefault(t => t.Surname == model.TeacherFullName.Split(" ")[0] &&
                                                                        t.Name == model.TeacherFullName.Split(" ")[1] &&
                                                                        t.Fathername == model.TeacherFullName.Split(" ")[2]).Id,
            };
            var id = subject.AreaId;
            db.Subjects.Update(subject);
            return RedirectToAction("Subjects", new { id });
        }
        [HttpPost]
        public IActionResult DeleteSubject(int id)
        {
            var sub = db.Subjects.Get(id.ToString());
            db.Subjects.Delete(id.ToString());
            id = sub.AreaId;
            return RedirectToAction("Subjects", new { id });
        }
    }
}
