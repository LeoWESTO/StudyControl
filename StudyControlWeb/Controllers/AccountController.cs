using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyControlWeb.Data;
using StudyControlWeb.Models;
using StudyControlWeb.ViewModels;
using System.Security.Claims;

namespace StudyControlWeb.Controllers
{
    public class AccountController : Controller
    {
        private DataContext db;
        public AccountController(DataContext context)
        {
            db = context;
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
                var tea = await db.Teacher.FirstOrDefaultAsync(t =>
                    t.Name == model.Name &&
                    t.Surname == model.Surname &&
                    t.Fathername == model.Fathername &&
                    t.Password == model.Password
                );
                if (tea != null)
                {
                    await Authenticate($"{model.Surname} {model.Name} {model.Fathername}", "Teacher"); // аутентификация
                    return RedirectToAction("Index", "Teacher");
                }
                var stu = await db.Student.FirstOrDefaultAsync(s =>
                    s.Name == model.Name &&
                    s.Surname == model.Surname &&
                    s.Fathername == model.Fathername &&
                    s.Password == model.Password
                );
                if (stu != null)
                {
                    await Authenticate($"{model.Surname} {model.Name} {model.Fathername}", "Student"); // аутентификация
                    return RedirectToAction("Index", "Student");
                }
                ModelState.AddModelError("", "Некорректные ФИО и(или) пароль");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult LoginStruct()
        {
            ViewBag.Titles = new SelectList(db.Faculty.ToList(), "Title", "Title");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginStruct(LoginStructModel model)
        {
            if (ModelState.IsValid)
            {
                var fac = await db.Faculty.FirstOrDefaultAsync(f =>
                    f.Title == model.Title &&
                    f.Password == model.Password
                );
                if (fac != null)
                {
                    await Authenticate(model.Title, "Faculty"); // аутентификация
                    return RedirectToAction("Index", "Faculty");
                }
                var dep = await db.Department.FirstOrDefaultAsync(d =>
                    d.Title == model.Title &&
                    d.Password == model.Password
                );
                if (dep != null)
                {
                    await Authenticate(model.Title, "Department"); // аутентификация
                    return RedirectToAction("Index", "Department");
                }
                ModelState.AddModelError("", "Некорректный пароль");
            }
            ViewBag.Titles = new SelectList(db.Faculty.ToList(), "Title", "Title");
            return View(model);
        }
        private async Task Authenticate(string userName, string role)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
