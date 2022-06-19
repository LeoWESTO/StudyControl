using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyControlWeb.Data;
using StudyControlWeb.Data.Repositories;
using StudyControlWeb.Models.DBO;
using StudyControlWeb.ViewModels;

namespace StudyControlWeb.Controllers
{
    [Authorize(Roles = "Admin, Faculty, Department, Student")]
    public class PerformanceController : Controller
    {
        private UniversityRepository db;
        public PerformanceController(DataContext context)
        {
            db = new UniversityRepository(context);
        }
        public IActionResult Attestations(int? groupId)
        {
            var model = new AttestationsViewModel();
            var currents = new List<CurrentAttestationViewModel>();
            var intermediates = new List<IntermediateAttestationViewModel>();
            var finales = new List<FinalAttestationViewModel>();
            if (User.IsInRole("Student"))
            {
                groupId = db.Students.Get(User.Identity.Name).GroupId;
                model.Group = db.Groups.Get(groupId.ToString());
                currents = db.CurrentAttestations.GetAll().
                    Where(c => c.StudentId.ToString() == User.Identity.Name).
                    Select(c => new CurrentAttestationViewModel()
                    {
                        Id = c.Id,
                        TermNumber = c.Subject.TermNumber,
                        StudentName = $"{c.Student.Surname} {c.Student.Name[0]}. {c.Student.Fathername[0]}.",
                        SubjectTitle = c.Subject.Title,
                        Performance = c.Performance,
                        Attendance = c.Attendance,
                        Date = c.Date,
                    }).ToList();
                intermediates = db.IntermediateAttestations.GetAll().
                    Where(i => i.StudentId.ToString() == User.Identity.Name).
                    Select(i => new IntermediateAttestationViewModel()
                    {
                        Id = i.Id,
                        TermNumber = i.Subject.TermNumber,
                        StudentName = $"{i.Student.Surname} {i.Student.Name[0]}. {i.Student.Fathername[0]}.",
                        SubjectTitle = i.Subject.Title,
                        Performance = i.Performance,
                        ControlType = i.ControlType,
                        Date = i.Date,
                    }).ToList();
                finales = db.FinalAttestations.GetAll().
                    Where(f => f.StudentId.ToString() == User.Identity.Name).
                    Select(f => new FinalAttestationViewModel()
                    {
                        Id = f.Id,
                        Type = f.Type,
                        StudentName = $"{f.Student.Surname} {f.Student.Name[0]}. {f.Student.Fathername[0]}.",
                        Performance = f.Performance,
                        Date = f.Date,
                    }).ToList();
            }
            else
            {
                model.Group = db.Groups.Get(groupId.ToString());
                currents = db.CurrentAttestations.GetAll().
                    Select(c => new CurrentAttestationViewModel()
                    {
                        Id = c.Id,
                        StudentName = $"{c.Student.Surname} {c.Student.Name[0]}. {c.Student.Fathername[0]}.",
                        SubjectTitle = c.Subject.Title,
                        Performance = c.Performance,
                        Attendance = c.Attendance,
                        TermNumber = c.Subject.TermNumber,
                        Date = c.Date,
                    }).ToList();
                intermediates = db.IntermediateAttestations.GetAll().
                    Select(i => new IntermediateAttestationViewModel()
                    {
                        Id = i.Id,
                        StudentName = $"{i.Student.Surname} {i.Student.Name[0]}. {i.Student.Fathername[0]}.",
                        SubjectTitle = i.Subject.Title,
                        Performance = i.Performance,
                        ControlType = i.ControlType,
                        TermNumber = i.Subject.TermNumber,
                        Date = i.Date,
                    }).ToList();
                finales = db.FinalAttestations.GetAll().
                    Select(f => new FinalAttestationViewModel()
                    {
                        Id = f.Id,
                        Type = f.Type,
                        StudentName = $"{f.Student.Surname} {f.Student.Name[0]}. {f.Student.Fathername[0]}.",
                        Performance = f.Performance,
                        Date = f.Date,
                    }).ToList();
            }
            
            model.CurrentAttestations = currents.OrderBy(c => c.StudentName).ThenBy(c => c.Date).ToList();
            model.IntermediateAttestations = intermediates.OrderBy(c => c.StudentName).ThenBy(c => c.Date).ToList();
            model.FinalAttestations = finales.OrderBy(c => c.StudentName).ThenBy(c => c.Date).ToList();
            return View(model);
        }
        public IActionResult CreateCurrentAttestation(int groupId)
        {
            var group = db.Groups.Get(groupId.ToString());

            ViewBag.GroupId = groupId;
            ViewBag.StudentNames = group.Students.Select(s => new SelectListItem { Text = $"{s.Surname} {s.Name} {s.Fathername}", Value = $"{s.Surname} {s.Name} {s.Fathername}" }); ;
            ViewBag.SubjectTitles = group.Area.Subjects.Select(s => new SelectListItem { Text = s.Title, Value = s.Title });
            return View();
        }
        [HttpPost]
        public IActionResult CreateCurrentAttestation(CurrentAttestationViewModel model, int groupId)
        {
            var group = db.Groups.Get(groupId.ToString());
            var currentAttestation = new CurrentAttestation()
            {
                StudentId = group.Students.FirstOrDefault(s => s.Surname == model.StudentName.Split(" ")[0] &&
                                                                        s.Name == model.StudentName.Split(" ")[1] &&
                                                                        s.Fathername == model.StudentName.Split(" ")[2]).Id,
                SubjectId = group.Area.Subjects.FirstOrDefault(s => s.Title == model.SubjectTitle).Id,
                Performance = model.Performance,
                Attendance = model.Attendance,
                Date = model.Date,
            };

            db.CurrentAttestations.Add(currentAttestation);
            return RedirectToAction("Attestations", new { groupId });
        }
        public IActionResult EditCurrentAttestation(int? id, int groupId)
        {
            if (id != null)
            {
                var ca = db.CurrentAttestations.Get(id.ToString());
                var group = db.Groups.Get(groupId.ToString());
                if (ca != null)
                {
                    var model = new CurrentAttestationViewModel()
                    {
                        Id = ca.Id,
                        StudentName = $"{ca.Student.Surname} {ca.Student.Name[0]}. {ca.Student.Fathername[0]}.",
                        SubjectTitle = ca.Subject.Title,
                        Performance = ca.Performance,
                        Attendance = ca.Attendance,
                        Date = ca.Date,
                    };

                    ViewBag.GroupId = groupId;
                    ViewBag.StudentNames = group.Students.Select(s => new SelectListItem { Text = $"{s.Surname} {s.Name} {s.Fathername}", Value = $"{s.Surname} {s.Name} {s.Fathername}" }); ;
                    ViewBag.SubjectTitles = group.Area.Subjects.Select(s => new SelectListItem { Text = s.Title, Value = s.Title });

                    return View(model);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult EditCurrentAttestation(CurrentAttestationViewModel model, int groupId)
        {
            var group = db.Groups.Get(groupId.ToString());
            var currentAttestation = new CurrentAttestation()
            {
                Id = model.Id,
                StudentId = group.Students.FirstOrDefault(s => s.Surname == model.StudentName.Split(" ")[0] &&
                                                                        s.Name == model.StudentName.Split(" ")[1] &&
                                                                        s.Fathername == model.StudentName.Split(" ")[2]).Id,
                SubjectId = group.Area.Subjects.FirstOrDefault(s => s.Title == model.SubjectTitle).Id,
                Performance = model.Performance,
                Attendance = model.Attendance,
                Date = model.Date,
            };

            db.CurrentAttestations.Update(currentAttestation);
            return RedirectToAction("Attestations", new { groupId });
        }
        [HttpPost]
        public IActionResult DeleteCurrentAttestation(int id)
        {
            var groupId = db.CurrentAttestations.Get(id.ToString()).Student.GroupId;

            db.CurrentAttestations.Delete(id.ToString());
            return RedirectToAction("Attestations", new { groupId });
        }
        public IActionResult CreateIntermediateAttestation(int groupId)
        {
            var group = db.Groups.Get(groupId.ToString());

            ViewBag.GroupId = groupId;
            ViewBag.StudentNames = group.Students.Select(s => new SelectListItem { Text = $"{s.Surname} {s.Name} {s.Fathername}", Value = $"{s.Surname} {s.Name} {s.Fathername}" }); ;
            ViewBag.SubjectTitles = group.Area.Subjects.Select(s => new SelectListItem { Text = s.Title, Value = s.Title });
            return View();
        }
        [HttpPost]
        public IActionResult CreateIntermediateAttestation(IntermediateAttestationViewModel model, int groupId)
        {
            var group = db.Groups.Get(groupId.ToString());
            var sub = group.Area.Subjects.FirstOrDefault(s => s.Title == model.SubjectTitle);
            var intermediateAttestation = new IntermediateAttestation()
            {
                StudentId = group.Students.FirstOrDefault(s => s.Surname == model.StudentName.Split(" ")[0] &&
                                                                        s.Name == model.StudentName.Split(" ")[1] &&
                                                                        s.Fathername == model.StudentName.Split(" ")[2]).Id,
                SubjectId = sub.Id,
                Performance = model.Performance,
                ControlType = sub.ControlTypes.Length > 2 ? sub.ControlTypes[0..(sub.ControlTypes.Length - 2)] : "",
                Date = model.Date,
            };

            db.IntermediateAttestations.Add(intermediateAttestation);
            return RedirectToAction("Attestations", new { groupId });
        }
        public IActionResult EditIntermediateAttestation(int? id, int groupId)
        {
            if (id != null)
            {
                var ia = db.IntermediateAttestations.Get(id.ToString());
                var group = db.Groups.Get(groupId.ToString());
                if (ia != null)
                {
                    var model = new IntermediateAttestationViewModel()
                    {
                        Id = ia.Id,
                        StudentName = $"{ia.Student.Surname} {ia.Student.Name[0]}. {ia.Student.Fathername[0]}.",
                        SubjectTitle = ia.Subject.Title,
                        Performance = ia.Performance,
                        Date = ia.Date,
                    };

                    ViewBag.GroupId = groupId;
                    ViewBag.StudentNames = group.Students.Select(s => new SelectListItem { Text = $"{s.Surname} {s.Name} {s.Fathername}", Value = $"{s.Surname} {s.Name} {s.Fathername}" }); ;
                    ViewBag.SubjectTitles = group.Area.Subjects.Select(s => new SelectListItem { Text = s.Title, Value = s.Title });

                    return View(model);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult EditIntermediateAttestation(IntermediateAttestationViewModel model, int groupId)
        {
            var group = db.Groups.Get(groupId.ToString());
            var intermediateAttestation = new IntermediateAttestation()
            {
                Id = model.Id,
                StudentId = group.Students.FirstOrDefault(s => s.Surname == model.StudentName.Split(" ")[0] &&
                                                                        s.Name == model.StudentName.Split(" ")[1] &&
                                                                        s.Fathername == model.StudentName.Split(" ")[2]).Id,
                SubjectId = group.Area.Subjects.FirstOrDefault(s => s.Title == model.SubjectTitle).Id,
                Performance = model.Performance,
                Date = model.Date,
            };

            db.IntermediateAttestations.Update(intermediateAttestation);
            return RedirectToAction("Attestations", new { groupId });
        }
        [HttpPost]
        public IActionResult DeleteIntermediateAttestation(int id)
        {
            var groupId = db.IntermediateAttestations.Get(id.ToString()).Student.GroupId;

            db.IntermediateAttestations.Delete(id.ToString());
            return RedirectToAction("Attestations", new { groupId });
        }
        public IActionResult CreateFinalAttestation(int groupId)
        {
            var group = db.Groups.Get(groupId.ToString());

            ViewBag.GroupId = groupId;
            ViewBag.StudentNames = group.Students.Select(s => new SelectListItem { Text = $"{s.Surname} {s.Name} {s.Fathername}", Value = $"{s.Surname} {s.Name} {s.Fathername}" }); ;
            ViewBag.Types = new List<string>() { "Препдипломная практика", "Госэкзамен", "ВКР" }.Select(s => new SelectListItem { Text = s, Value = s });
            return View();
        }
        [HttpPost]
        public IActionResult CreateFinalAttestation(FinalAttestationViewModel model, int groupId)
        {
            var group = db.Groups.Get(groupId.ToString());
            var finalAttestation = new FinalAttestation()
            {
                StudentId = group.Students.FirstOrDefault(s => s.Surname == model.StudentName.Split(" ")[0] &&
                                                                        s.Name == model.StudentName.Split(" ")[1] &&
                                                                        s.Fathername == model.StudentName.Split(" ")[2]).Id,
                Performance = model.Performance,
                Type = model.Type,
                Date = model.Date,
            };

            db.FinalAttestations.Add(finalAttestation);
            return RedirectToAction("Attestations", new { groupId });
        }
        public IActionResult EditFinalAttestation(int? id, int groupId)
        {
            if (id != null)
            {
                var fa = db.FinalAttestations.Get(id.ToString());
                var group = db.Groups.Get(groupId.ToString());
                if (fa != null)
                {
                    var model = new FinalAttestationViewModel()
                    {
                        Id = fa.Id,
                        Performance = fa.Performance,
                        Type = fa.Type,
                        Date = fa.Date,
                    };

                    ViewBag.GroupId = groupId;
                    ViewBag.StudentNames = group.Students.Select(s => new SelectListItem { Text = $"{s.Surname} {s.Name} {s.Fathername}", Value = $"{s.Surname} {s.Name} {s.Fathername}" }); ;
                    ViewBag.Types = new List<string>() { "Препдипломная практика", "Госэкзамен", "ВКР" }.Select(s => new SelectListItem { Text = s, Value = s });

                    return View(model);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult EditFinalAttestation(FinalAttestationViewModel model, int groupId)
        {
            var group = db.Groups.Get(groupId.ToString());
            var finalAttestation = new FinalAttestation()
            {
                Id = model.Id,
                StudentId = group.Students.FirstOrDefault(s => s.Surname == model.StudentName.Split(" ")[0] &&
                                                                        s.Name == model.StudentName.Split(" ")[1] &&
                                                                        s.Fathername == model.StudentName.Split(" ")[2]).Id,
                Performance = model.Performance,
                Type = model.Type,
                Date = model.Date,
            };

            db.FinalAttestations.Update(finalAttestation);
            return RedirectToAction("Attestations", new { groupId });
        }
        [HttpPost]
        public IActionResult DeleteFinalAttestation(int id)
        {
            var groupId = db.FinalAttestations.Get(id.ToString()).Student.GroupId;

            db.FinalAttestations.Delete(id.ToString());
            return RedirectToAction("Attestations", new { groupId });
        }
    }
}
