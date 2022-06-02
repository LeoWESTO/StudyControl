using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyControlWeb.Data;
using StudyControlWeb.Data.Repositories;

namespace StudyControlWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AreaController : Controller
    {
        private UniversityRepository db;
        public AreaController(DataContext context)
        {
            db = new UniversityRepository(context);
        }
        public IActionResult Areas()
        {
            return View(db.Areas.GetAll());
        }
    }
}
