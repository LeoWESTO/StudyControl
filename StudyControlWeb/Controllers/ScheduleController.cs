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
        public IActionResult Schedules(string search, SortState sortState = SortState.FirstAsc, int page = 1)
        {
            IEnumerable<ScheduleViewModel> model = new List<ScheduleViewModel>();
            if (User.IsInRole("Student"))
            {
                var groupId = db.Students.Get(User.Identity.Name).GroupId;
                model = db.Schedules.GetAll().
                    Where(s => s.GroupId == groupId).
                    Select(s => new ScheduleViewModel()
                    {
                        Id = s.Id,
                        GroupCode = s.Group.Code,
                        FacultyTitle = string.Join(string.Empty, s.Faculty.Title.Split(' ', '-').Select(s => char.ToUpper(s[0]))),
                        TermNumber = s.TermNumber,
                    });
            }
            if (User.IsInRole("Admin"))
            {
                model = db.Schedules.GetAll().
                    Select(s => new ScheduleViewModel()
                    {
                        Id = s.Id,
                        GroupCode = s.Group.Code,
                        FacultyTitle = string.Join(string.Empty, s.Faculty.Title.Split(' ', '-').Select(s => char.ToUpper(s[0]))),
                        TermNumber = s.TermNumber,
                    });
            }
            if (User.IsInRole("Faculty"))
            {
                model = db.Schedules.GetAll().
                    Where(s => s.FacultyId.ToString() == User.Identity.Name).
                    Select(s => new ScheduleViewModel()
                    {
                        Id = s.Id,
                        GroupCode = s.Group.Code,
                        FacultyTitle = string.Join(string.Empty, s.Faculty.Title.Split(' ', '-').Select(s => char.ToUpper(s[0]))),
                        TermNumber = s.TermNumber,
                    });
            }
            if (User.IsInRole("Department"))
            {
                model = db.Schedules.GetAll().
                    Where(s => s.Group.DepartmentId.ToString() == User.Identity.Name).
                    Select(s => new ScheduleViewModel()
                    {
                        Id = s.Id,
                        GroupCode = s.Group.Code,
                        FacultyTitle = string.Join(string.Empty, s.Faculty.Title.Split(' ', '-').Select(s => char.ToUpper(s[0]))),
                        TermNumber = s.TermNumber,
                    });
            }

            //фильтрация
            if (!String.IsNullOrEmpty(search))
            {
                model = model.Where(f => f.GroupCode.ToUpper().Contains(search.ToUpper()));
                ViewBag.Search = search;
            }

            //сортировка
            model = sortState switch
            {
                SortState.FirstAsc => model.OrderBy(s => s.TermNumber),
                SortState.FirstDesc => model.OrderByDescending(s => s.TermNumber),
                SortState.SecondAsc => model.OrderBy(s => s.GroupCode),
                SortState.SecondDesc => model.OrderByDescending(s => s.GroupCode),
                SortState.ThirdAsc => model.OrderBy(s => s.FacultyTitle),
                SortState.ThirdDesc => model.OrderByDescending(s => s.FacultyTitle),
                _ => model.OrderBy(s => s.TermNumber),
            };
            ViewBag.SortModel = new SortViewModel(sortState);

            //пагинация
            int count = model.Count();
            int pageSize = 10;
            model = model.Skip((page - 1) * pageSize).Take(pageSize);
            ViewBag.PageModel = new PageViewModel(count, page, pageSize);

            return View(model);
        }
        [Authorize(Roles = "Admin, Faculty")]
        public IActionResult CreateSchedule()
        {
            ViewBag.Groups = GroupCodes();
            ViewBag.Faculties = FacultyTitles();

            return View();
        }
        [Authorize(Roles = "Admin, Faculty")]
        [HttpPost]
        public IActionResult CreateSchedule(ScheduleViewModel model)
        {
            var schedule = new Schedule()
            {
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
                    if (User.IsInRole("Faculty") && schedule.FacultyId.ToString() != User.Identity.Name) return NotFound();
                    var model = new ScheduleViewModel()
                    {
                        Id = schedule.Id,
                        GroupCode = schedule.Group.Code,
                        FacultyTitle = schedule.Faculty.Title,
                        TermNumber = schedule.TermNumber,
                    };
                    ViewBag.Groups = GroupCodes();
                    ViewBag.Faculties = FacultyTitles();

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
            if (User.IsInRole("Faculty") && db.Schedules.Get(id.ToString()).FacultyId.ToString() != User.Identity.Name) return NotFound();
            db.Schedules.Delete(id.ToString());
            return RedirectToAction("Schedules");
        }
        [Authorize(Roles = "Admin, Faculty")]
        [HttpPost]
        public IActionResult CopySchedule(int id)
        {
            var schedule = db.Schedules.Get(id.ToString());
            if (schedule != null)
            {
                if (User.IsInRole("Faculty") && schedule.FacultyId.ToString() != User.Identity.Name) return NotFound();

                var copySchedule = new Schedule()
                {
                    TermNumber = schedule.TermNumber,
                    FacultyId = schedule.FacultyId,
                    GroupId = schedule.GroupId,
                };
                db.Schedules.Add(copySchedule);
                return RedirectToAction("Schedules");
            }
            return NotFound();
        }

        private IEnumerable<SelectListItem> FacultyTitles()
        {
            IEnumerable<SelectListItem> list = new List<SelectListItem>();
            if (User.IsInRole("Admin"))
            {
                list = db.Faculties.GetAll().Select(f => f.Title).Select(t => new SelectListItem { Text = t, Value = t });
            }
            if (User.IsInRole("Faculty"))
            {
                list = db.Faculties.GetAll().Where(f => f.Id.ToString() == User.Identity.Name).Select(f => f.Title).Select(t => new SelectListItem { Text = t, Value = t });
            }
            return list;
        }
        private IEnumerable<SelectListItem> GroupCodes()
        {
            IEnumerable<SelectListItem> list = new List<SelectListItem>();
            if (User.IsInRole("Admin"))
            {
                list = db.Groups.GetAll().Select(g => g.Code).Select(t => new SelectListItem { Text = t, Value = t });
            }
            if (User.IsInRole("Faculty"))
            {
                list = db.Groups.GetAll().Where(g => g.FacultyId.ToString() == User.Identity.Name).Select(g => g.Code).Select(t => new SelectListItem { Text = t, Value = t });
            }
            return list;
        }
    }
}
