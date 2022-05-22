using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyControlWeb.Data;
using StudyControlWeb.ViewModels;
using System.Security.Claims;

namespace StudyControlWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private DataContext db;
        public AdminController(DataContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
