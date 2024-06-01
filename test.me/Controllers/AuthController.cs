using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.Intrinsics.X86;
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

        public IActionResult Index()
        {
            if(!db.Users.Any(x=>x.Username == "admin"))
            {
                testme.Models.User user1 = new testme.Models.User("admin", "admin", UserType.ADMIN);
                db.Users.Add(user1);
                db.SaveChanges();
            }

            if (Tools.isSessionActual(_cache)) return Redirect("Home");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            var currentUser = testme.Models.User.getUser(username, password);
            if (currentUser == null)
            {
                ViewBag.Message = "Неправильный логин или пароль";
                return View();
            }

            Session newSession = new Session(currentUser.Id);

            foreach (var item in db.Sessions)
            {
                if (item.UserId == currentUser.Id)
                    item.isActual = false;
            }

            db.Sessions.Add(newSession);
            db.SaveChanges();
            
            _cache.Set("sessionId", newSession.SessionID.ToString());

            return Redirect("Home");
        }

        public IActionResult Logout()
        {
            _cache.Remove("sessionId");
            return Redirect("Auth");
        }
    }
}