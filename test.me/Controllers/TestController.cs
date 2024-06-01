using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using testme.Models;

namespace testme
{
    public class TestController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationContext db;

        public TestController(IMemoryCache cache)
        {
            _cache = cache;
            db = new ApplicationContext();
        }
        public IActionResult AllTests(string search)
        {
            if (!Tools.isSessionActual(_cache)) return Redirect("../Auth");
            var currentUser = Tools.getCurrentUser(_cache);
            ViewBag.Username = currentUser.Username;

            if (search == null)
                return View(db.Tests.ToList());
            else
                return View(db.Tests.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList());
        }

        public IActionResult Index(int id) {

            if (!Tools.isSessionActual(_cache)) return Redirect("Auth");
            var currentUser = Tools.getCurrentUser(_cache);
            if (currentUser.UserType != UserType.STUDENT) return Unauthorized();
            ViewBag.Username = currentUser.Username;

            Test currentTest = db.Tests.FirstOrDefault(x => x.Id == id);
            if (currentTest == null)
                return NotFound();

            currentTest.Questions = db.Questions.Where(x => x.TestId == currentTest.Id).ToList();
            foreach(var item in currentTest.Questions)
            {
                item.Answers = db.Answers.Where(x => x.QuestionId == item.Id).ToList();
            }
            return View(currentTest);
        }

        
        public IActionResult Check(int testId, string sessionId, Dictionary<int, int> answers)
        {
            if (!Tools.isSessionActual(_cache)) return Redirect("Auth");
            var currentUser = Tools.getCurrentUser(_cache);
            if (currentUser.UserType != UserType.STUDENT) return Unauthorized();

            ViewBag.Username = currentUser.Username;

            return Content(string.Join(" ", answers));
        }

        public IActionResult MyTests(int userId, string search)
        {
            if (!Tools.isSessionActual(_cache)) return Redirect("../Auth");
            var currentUser = Tools.getCurrentUser(_cache);
            if (userId != currentUser.Id) return Unauthorized();
            ViewBag.Username = currentUser.Username;
            ViewBag.UserId = currentUser.Id;

            var testOfCurrentUser = db.Tests.Where(x => x.CreatorId == currentUser.Id);

            if (search == null)
                return View(testOfCurrentUser);
            else
                return View(testOfCurrentUser.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string[] questions, int[] answersCount, string[] answers)
        {
            if (!Tools.isSessionActual(_cache)) return Redirect("../Auth");
            var currentUser = Tools.getCurrentUser(_cache);
            if (currentUser.UserType != UserType.TEACHER) return Unauthorized();

            

            return Content(string.Join(" ", questions));
        }


        public IActionResult Create()
        {
            if (!Tools.isSessionActual(_cache)) return Redirect("../Auth");
            var currentUser = Tools.getCurrentUser(_cache);
            if (currentUser.UserType != UserType.TEACHER) return Unauthorized();

            return View();
        }
    }
}