﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudyControlWeb.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class FacultyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
