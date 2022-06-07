using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyControlWeb.Data;
using StudyControlWeb.Data.Repositories;
using StudyControlWeb.Models.DBO;
using StudyControlWeb.ViewModels;

namespace StudyControlWeb.Controllers
{
    [Authorize]
    public class ScheduleController : Controller
    {
        private UniversityRepository db;
        public ScheduleController(DataContext context)
        {
            db = new UniversityRepository(context);
        }
        public IActionResult Schedules()
        {
            var model = db.Schedules.GetAll().
                Select(s => new ScheduleViewModel()
                {
                    Id = s.Id,
                    GroupCode = s.Group.Code,
                    FacultyTitle = string.Join(string.Empty, s.Faculty.Title.Split(' ', '-').Select(s => char.ToUpper(s[0]))),
                    TermNumber = s.TermNumber,
                });
            return View(model);
        }
        [Authorize(Roles = "Admin, Faculty")]
        public IActionResult CreateSchedule()
        {
            ViewBag.Groups = db.Groups.GetAll().Select(g => g.Code).Select(t => new SelectListItem { Text = t, Value = t });
            ViewBag.Faculties = db.Faculties.GetAll().Select(f => f.Title).Select(t => new SelectListItem { Text = t, Value = t });
            return View();
        }
        [Authorize(Roles = "Admin, Faculty")]
        [HttpPost]
        public IActionResult CreateSchedule(ScheduleViewModel model)
        {
            var schedule = new Schedule()
            {
                Id = model.Id,
                TermNumber = model.TermNumber,
                FacultyId = db.Faculties.GetAll().FirstOrDefault(f => f.Title == model.FacultyTitle).Id,
                GroupId = db.Groups.GetAll().FirstOrDefault(g => g.Code == model.GroupCode).Id,
            };
            db.Schedules.Add(schedule);
            return RedirectToAction("Schedules");
        }
        [Authorize(Roles = "Admin, Faculty")]
        public IActionResult EditSchedule(int? id)
        {
            if (id != null)
            {
                Schedule? schedule = db.Schedules.Get(id.ToString());
                if (schedule != null)
                {
                    var model = new ScheduleViewModel()
                    {
                        Id = schedule.Id,
                        GroupCode = schedule.Group.Code,
                        FacultyTitle = schedule.Faculty.Title,
                        TermNumber = schedule.TermNumber,
                    };
                    ViewBag.Groups = db.Groups.GetAll().Select(g => g.Code).Select(t => new SelectListItem { Text = t, Value = t });
                    ViewBag.Faculties = db.Faculties.GetAll().Select(f => f.Title).Select(t => new SelectListItem { Text = t, Value = t });
                    return View(model);
                }
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin, Faculty")]
        [HttpPost]
        public IActionResult EditSchedule(ScheduleViewModel model)
        {
            var schedule = new Schedule()
            {
                Id = model.Id,
                TermNumber = model.TermNumber,
                FacultyId = db.Faculties.GetAll().FirstOrDefault(f => f.Title == model.FacultyTitle).Id,
                GroupId = db.Groups.GetAll().FirstOrDefault(g => g.Code == model.GroupCode).Id,
            };

            db.Schedules.Update(schedule);
            return RedirectToAction("Schedules");
        }
        [Authorize(Roles = "Admin, Faculty")]
        [HttpPost]
        public IActionResult DeleteSchedule(int id)
        {
            db.Schedules.Delete(id.ToString());
            return RedirectToAction("Schedules");
        }
    }
}
