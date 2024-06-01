using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using testme.Models;

namespace testme.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationContext db;

        public AccountController(IMemoryCache cache)
        {
            _cache = cache;
            db = new ApplicationContext();
        }

        public IActionResult Index()
        {
            if (!Tools.isSessionActual(_cache)) return Redirect("../Auth");
            var currentUser = Tools.getCurrentUser(_cache);

            ViewBag.Username = currentUser.Username;
            ViewBag.UserId = currentUser.Id;

            switch (currentUser.UserType)
            {
                case UserType.ADMIN:
                    return View("AdminAccount");
                case UserType.TEACHER:
                    return View("TeacherAccount");
                default:
                    return View("StudentAccount");

            }
        }        
    }
}
