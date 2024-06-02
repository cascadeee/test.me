using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using testme.Models;

namespace testme.Controllers
{
    public class UserController : Controller
    {

        private readonly IMemoryCache _cache;
        private readonly ApplicationContext db;

        public UserController(IMemoryCache cache)
        {
            _cache = cache;
            db = new ApplicationContext();
        }

        public IActionResult Create()
        {
            if (!Tools.isSessionActual(_cache)) return Redirect("../Auth");
            var currentUser = Tools.getCurrentUser(_cache);
            if (currentUser.UserType != UserType.ADMIN) return Unauthorized();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string username, string password, string usertype)
        {
            if (!Tools.isSessionActual(_cache)) return Redirect("../Auth");
            var currentUser = Tools.getCurrentUser(_cache);
            if (currentUser.UserType != UserType.ADMIN) return Unauthorized();

            if (db.Users.Any(x => x.Username == username))
                return Content("Пользователь с таким именем уже существует");

            UserType type;
            switch (usertype)
            {
                case "ADMIN":
                    type = UserType.ADMIN;
                    break;
                case "TEACHER":
                    type = UserType.TEACHER;
                    break;
                default:
                    type = UserType.STUDENT;
                    break;
            }

            User user = new User(username, password, type);
            db.Users.Add(user);
            db.SaveChanges();

            return RedirectToAction("AllUsers");
        }

        public IActionResult AllUsers(string search)
        {
            if (!Tools.isSessionActual(_cache)) return Redirect("../Auth");
            var currentUser = Tools.getCurrentUser(_cache);
            if (currentUser.UserType != UserType.ADMIN) return Unauthorized();
            ViewBag.Username = currentUser.Username;

            if (search == null)
                return View(db.Users.ToList());
            else
                return View(db.Users.Where(x => x.Username.ToLower().Contains(search.ToLower())).ToList());
        }

        public IActionResult Delete(int id)
        {
            if (!Tools.isSessionActual(_cache)) return Redirect("../Auth");
            var currentUser = Tools.getCurrentUser(_cache);
            if (currentUser.UserType != UserType.ADMIN) return Unauthorized();

            if (id == currentUser.Id) return Content("Вы не можете удалить себя.");

            User userToDelete = db.Users.FirstOrDefault(x => x.Id == id);
            if (userToDelete == null) return NotFound();


            db.Users.Remove(userToDelete);
            db.SaveChanges();

            return RedirectToAction("AllUsers");
        }
    }
}
