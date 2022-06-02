using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyControlWeb.Data;
using StudyControlWeb.Data.Repositories;
using StudyControlWeb.Models;
using StudyControlWeb.ViewModels;
using System.Security.Claims;

namespace StudyControlWeb.Controllers
{
    public class AccountController : Controller
    {
        private UniversityRepository db;
        public AccountController(DataContext context)
        {
            db = new UniversityRepository(context);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var tea = db.Teachers.GetAll().FirstOrDefault(t =>
                    t.Name == model.Name &&
                    t.Surname == model.Surname &&
                    t.Fathername == model.Fathername &&
                    t.Password == model.Password
                );
                if (tea != null)
                {
                    await Authenticate(tea.Id.ToString(), "Teacher"); // аутентификация
                    return RedirectToAction("Index", "Home");
                }
                var stu = db.Students.GetAll().FirstOrDefault(s =>
                    s.Name == model.Name &&
                    s.Surname == model.Surname &&
                    s.Fathername == model.Fathername &&
                    s.Password == model.Password
                );
                if (stu != null)
                {
                    await Authenticate(stu.Id.ToString(), "Student"); // аутентификация
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные ФИО и(или) пароль");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult LoginStruct()
        {
            var list = db.Faculties.GetAll().Select(f => f.Title).ToList();
            list.AddRange(db.Departments.GetAll().Select(d => d.Title).ToList());
            ViewBag.Titles = list.Select(l => new SelectListItem { Text = l, Value = l });
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginStruct(LoginStructModel model)
        {
            if (ModelState.IsValid)
            {
                var fac = db.Faculties.GetAll().FirstOrDefault(f =>
                    f.Title == model.Title &&
                    f.Password == model.Password
                );
                if (fac != null)
                {
                    await Authenticate(fac.Id.ToString(), "Faculty"); // аутентификация
                    return RedirectToAction("Index", "Home");
                }
                var dep = db.Departments.GetAll().FirstOrDefault(d =>
                    d.Title == model.Title &&
                    d.Password == model.Password
                );
                if (dep != null)
                {
                    await Authenticate(dep.Id.ToString(), "Department"); // аутентификация
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректный пароль");
            }
            ViewBag.Titles = new SelectList(db.Faculties.GetAll(), "Title", "Title");
            return View(model);
        }
        public IActionResult LoginAdmin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAdmin(LoginAdminModel model)
        {
            if (ModelState.IsValid)
            {
                var adm = db.Admins.GetAll().FirstOrDefault(a =>
                    a.Login == model.Login &&
                    a.Password == model.Password
                );
                if (adm != null)
                {
                    await Authenticate(adm.Id.ToString(), "Admin"); // аутентификация
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        private async Task Authenticate(string userName, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
