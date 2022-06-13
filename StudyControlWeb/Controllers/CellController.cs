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
    public class CellController : Controller
    {
        private UniversityRepository db;
        public CellController(DataContext context)
        {
            db = new UniversityRepository(context);
        }
        public IActionResult Cells(int scheduleId)
        {
            ScheduleViewModel model = new ScheduleViewModel();
            var schedule = db.Schedules.Get(scheduleId.ToString());
            if (schedule != null)
            {
                model = new ScheduleViewModel()
                {
                    Id = scheduleId,
                    FacultyTitle = schedule.Faculty.Title,
                    GroupCode = schedule.Group.Code,
                    TermNumber = schedule.TermNumber,
                };
                var cells = db.Cells.GetAll().
                    Where(c => c.ScheduleId == scheduleId).
                    Select(c => new CellViewModel()
                    {
                        Id = c.Id,
                        ScheduleId = c.ScheduleId,
                        TeacherName = $"{c.Teacher.Surname} {c.Teacher.Name[0]}. {c.Teacher.Fathername[0]}.",
                        Classroom = c.Classroom,
                        SubjectTitle = c.Subject.Title,
                        LessonType = c.LessonType,
                        LessonNumber = c.LessonNumber,
                        DayOfWeek = c.DayOfWeek,
                        WeekNumber = c.WeekNumber,
                        ControlType = c.Subject.ControlTypes.Length > 2 ? c.Subject.ControlTypes[0..(c.Subject.ControlTypes.Length - 2)] : "",
                    });

                model.Cells = cells.OrderBy(c => c.WeekNumber).ToList();
            }
            return View(model);
        }
        [Authorize(Roles = "Admin, Faculty")]
        public IActionResult CreateCell(int scheduleId)
        {
            var schedule = db.Schedules.Get(scheduleId.ToString());
            ViewBag.Subjects = schedule.Group.Area.Subjects.Where(s => s.TermNumber == schedule.TermNumber).Select(s => s.Title).Select(t => new SelectListItem { Text = t, Value = t });
            ViewBag.LessonType = new List<string>() { "ЛК", "ПЗ", "ЛБ", "КР", "Консультация", "Экзамен" }.Select(t => new SelectListItem { Text = t, Value = t });
            ViewBag.ScheduleId = new List<SelectListItem>() { new SelectListItem { Text = scheduleId.ToString(), Value = scheduleId.ToString() } };
            return View();
        }
        [Authorize(Roles = "Admin, Faculty")]
        [HttpPost]
        public IActionResult CreateCell(CellViewModel model)
        {
            var cell = new Cell()
            {
                ScheduleId = model.ScheduleId,
                SubjectId = db.Schedules.Get(model.ScheduleId.ToString()).Group.Area.Subjects.FirstOrDefault(s => s.Title == model.SubjectTitle).Id,
                GroupId = db.Schedules.Get(model.ScheduleId.ToString()).Group.Id,
                TeacherId = (int)db.Schedules.Get(model.ScheduleId.ToString()).Group.Area.Subjects.FirstOrDefault(s => s.Title == model.SubjectTitle).TeacherId,
                LessonNumber = model.LessonNumber,
                LessonType = model.LessonType,
                DayOfWeek = model.DayOfWeek,
                WeekNumber = model.WeekNumber,
                Classroom = model.Classroom,
            };
            db.Cells.Add(cell);
            return RedirectToAction("Cells", new { scheduleId = model.ScheduleId });
        }
        [Authorize(Roles = "Admin, Faculty")]
        public IActionResult EditCell(int id)
        {
            Cell? cell = db.Cells.Get(id.ToString());
            if (cell != null)
            {
                var model = new CellViewModel()
                {
                    Id = cell.Id,
                    ScheduleId = cell.ScheduleId,
                    TeacherName = $"{cell.Teacher.Surname} {cell.Teacher.Name[0]}. {cell.Teacher.Fathername[0]}.",
                    Classroom = cell.Classroom,
                    SubjectTitle = cell.Subject.Title,
                    LessonType = cell.LessonType,
                    LessonNumber = cell.LessonNumber,
                    DayOfWeek = cell.DayOfWeek,
                    WeekNumber = cell.WeekNumber,
                };

                var schedule = db.Schedules.Get(cell.ScheduleId.ToString());
                ViewBag.Subjects = schedule.Group.Area.Subjects.Where(s => s.TermNumber == schedule.TermNumber).Select(s => s.Title).Select(t => new SelectListItem { Text = t, Value = t });
                ViewBag.LessonType = new List<string>() { "ЛК", "ПЗ", "ЛБ", "КР", "Консультация", "Экзамен" }.Select(t => new SelectListItem { Text = t, Value = t });
                ViewBag.ScheduleId = new List<SelectListItem>() { new SelectListItem { Text = cell.ScheduleId.ToString(), Value = cell.ScheduleId.ToString() } };

                return View(model);
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin, Faculty")]
        [HttpPost]
        public IActionResult EditCell(CellViewModel model)
        {
            var cell = new Cell()
            {
                Id = model.Id,
                ScheduleId = model.ScheduleId,
                SubjectId = db.Schedules.Get(model.ScheduleId.ToString()).Group.Area.Subjects.FirstOrDefault(s => s.Title == model.SubjectTitle).Id,
                GroupId = db.Schedules.Get(model.ScheduleId.ToString()).Group.Id,
                TeacherId = (int)db.Schedules.Get(model.ScheduleId.ToString()).Group.Area.Subjects.FirstOrDefault(s => s.Title == model.SubjectTitle).TeacherId,
                LessonNumber = model.LessonNumber,
                LessonType = model.LessonType,
                DayOfWeek = model.DayOfWeek,
                WeekNumber = model.WeekNumber,
                Classroom = model.Classroom,
            };

            db.Cells.Update(cell);
            return RedirectToAction("Cells", new { scheduleId = cell.ScheduleId });
        }
        [Authorize(Roles = "Admin, Faculty")]
        [HttpPost]
        public IActionResult DeleteCell(int id)
        {
            var cell = db.Cells.Get(id.ToString());
            db.Cells.Delete(id.ToString());
            return RedirectToAction("Cells", new { scheduleId = cell.ScheduleId });
        }
        [Authorize(Roles = "Admin, Faculty")]
        [HttpPost]
        public IActionResult CopyCell(int id)
        {
            var cell = db.Cells.Get(id.ToString());
            if (cell != null)
            {
                var copyCell = new Cell()
                {
                    ScheduleId = cell.ScheduleId,
                    SubjectId = cell.SubjectId,
                    GroupId = cell.GroupId,
                    TeacherId = cell.TeacherId,
                    LessonNumber = cell.LessonNumber + 1,
                    LessonType = cell.LessonType,
                    DayOfWeek = cell.DayOfWeek,
                    WeekNumber = cell.WeekNumber,
                    Classroom = cell.Classroom,
                };

                db.Cells.Add(copyCell);
                return RedirectToAction("Cells", new { scheduleId = cell.ScheduleId });
            }
            return NotFound();
        }
    }
}
