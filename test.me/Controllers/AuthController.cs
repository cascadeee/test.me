using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using testme.Models;

namespace testme
{
    public class AuthController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationContext db;

        public AuthController(IMemoryCache cache)
        {
            _cache = cache;
            db = new ApplicationContext();
        }

        public ViewResult Index()
        {
            //db.Users.Add(
            //    new User("admin", "123", UserType.ADMIN)
            //    );
            //db.SaveChanges();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            if (!testme.Models.User.isUsernameExists(username))
            {
                ViewBag.Message = "Неправильный логин или пароль";
                return View();
            }
            return Redirect("/Home");
        }

    }
}