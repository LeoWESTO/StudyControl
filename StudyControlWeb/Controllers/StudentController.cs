using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyControlWeb.Data;
using StudyControlWeb.Data.Repositories;
using StudyControlWeb.Models.DBO;
using StudyControlWeb.ViewModels;
using System.Security.Claims;

namespace StudyControlWeb.Controllers
{
    [Authorize(Roles = "Admin, Faculty, Department")]
    public class StudentController : Controller
    {
        private UniversityRepository db;
        public StudentController(DataContext context)
        {
            db = new UniversityRepository(context);
        }
        public IActionResult Students(int? groupId, string search, SortState sortState = SortState.FourthAsc, int page = 1)
        {
            IEnumerable<StudentViewModel> model = new List<StudentViewModel>();
            if (groupId != null)
            {
                var group = db.Groups.Get(groupId.ToString());
                if (group != null)
                {
                    model = group.Students.
                        Select(s => new StudentViewModel()
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Surname = s.Surname,
                            Fathername = s.Fathername,
                            GroupCode = s.Group.Code,
                            Year = DateTime.Now.Year - s.Group.StartDate.Year,
                            Password = s.Password
                        });
                }
            }
            else
            {
                if (User.IsInRole("Admin"))
                {
                    model = db.Students.GetAll().
                        Select(s => new StudentViewModel()
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Surname = s.Surname,
                            Fathername = s.Fathername,
                            GroupCode = s.Group.Code,
                            Year = DateTime.Now.Year - s.Group.StartDate.Year,
                            Password = s.Password
                        });
                }
                if (User.IsInRole("Faculty"))
                {
                    model = db.Students.GetAll().
                        Where(s => s.Group.Department.FacultyId.ToString() == User.Identity.Name).
                        Select(s => new StudentViewModel()
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Surname = s.Surname,
                            Fathername = s.Fathername,
                            GroupCode = s.Group.Code,
                            Year = DateTime.Now.Year - s.Group.StartDate.Year,
                            Password = s.Password
                        });
                }
                if (User.IsInRole("Department"))
                {
                    model = db.Students.GetAll().
                        Where(s => s.Group.DepartmentId.ToString() == User.Identity.Name).
                        Select(s => new StudentViewModel()
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Surname = s.Surname,
                            Fathername = s.Fathername,
                            GroupCode = s.Group.Code,
                            Year = DateTime.Now.Year - s.Group.StartDate.Year,
                            Password = s.Password
                        });
                }
            }

            //фильтрация
            if (!String.IsNullOrEmpty(search))
            {
                model = model.Where(f => (f.Surname + f.Name + f.Fathername).ToUpper().Contains(search.ToUpper()));
                ViewBag.Search = search;
            }

            //сортировка
            model = sortState switch
            {
                SortState.FirstAsc => model.OrderBy(s => s.Surname),
                SortState.FirstDesc => model.OrderByDescending(s => s.Surname),
                SortState.SecondAsc => model.OrderBy(s => s.Name),
                SortState.SecondDesc => model.OrderByDescending(s => s.Name),
                SortState.ThirdAsc => model.OrderBy(s => s.Fathername),
                SortState.ThirdDesc => model.OrderByDescending(s => s.Fathername),
                SortState.FourthAsc => model.OrderBy(s => s.GroupCode),
                SortState.FourthDesc => model.OrderByDescending(s => s.GroupCode),
                SortState.FifthAsc => model.OrderBy(s => s.Year),
                SortState.FifthDesc => model.OrderByDescending(s => s.Year),
                _ => model.OrderBy(s => s.GroupCode).ThenBy(s => s.Surname),
            };
            ViewBag.SortModel = new SortViewModel(sortState);

            //пагинация
            int count = model.Count();
            int pageSize = 10;
            model = model.Skip((page - 1) * pageSize).Take(pageSize);
            ViewBag.PageModel = new PageViewModel(count, page, pageSize);

            return View(model);
        }
        public IActionResult CreateStudent()
        {
            ViewBag.Titles = Titles();
            return View();
        }
        [HttpPost]
        public IActionResult CreateStudent(StudentViewModel model)
        {
            var student = new Student()
            {
                Id = model.Id,
                Name = model.Name,
                Surname = model.Surname,
                Fathername = model.Fathername,
                GroupId = db.Groups.GetAll().FirstOrDefault(g => g.Code == model.GroupCode).Id,
                Password = model.Password,
            };

            db.Students.Add(student);
            return RedirectToAction("Students");
        }
        public IActionResult EditStudent(int? id)
        {
            if (id != null)
            {
                Student? stu = db.Students.Get(id.ToString());
                if (stu != null)
                {
                    var model = new StudentViewModel()
                    {
                        Id = stu.Id,
                        Name = stu.Name,
                        Surname = stu.Surname,
                        Fathername = stu.Fathername,
                        GroupCode = stu.Group.Code,
                        Password = stu.Password
                    };
                    ViewBag.Titles = Titles();

                    return View(model);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult EditStudent(StudentViewModel model)
        {
            var student = new Student()
            {
                Id = model.Id,
                Name = model.Name,
                Surname = model.Surname,
                Fathername = model.Fathername,
                GroupId = db.Groups.GetAll().FirstOrDefault(g => g.Code == model.GroupCode).Id,
                Password = model.Password,
            };

            db.Students.Update(student);
            return RedirectToAction("Students");
        }
        [HttpPost]
        public IActionResult DeleteStudent(int? id)
        {
            if (id != null)
            {
                db.Students.Delete(id.ToString());
                return RedirectToAction("Students");
            }
            return NotFound();
        }

        private IEnumerable<SelectListItem> Titles()
        {
            IEnumerable<string> titles = new List<string>();
            if (User.IsInRole("Admin"))
            {
                titles = db.Groups.GetAll().Select(g => g.Code);
            }
            if (User.IsInRole("Faculty"))
            {
                titles = db.Groups.GetAll().Where(g => g.Department.FacultyId.ToString() == User.Identity.Name).Select(g => g.Code);
            }
            if (User.IsInRole("Department"))
            {
                titles = db.Groups.GetAll().Where(g => g.DepartmentId.ToString() == User.Identity.Name).Select(g => g.Code);
            }

            return titles.Select(t => new SelectListItem { Text = t, Value = t });
        }
    }
}
