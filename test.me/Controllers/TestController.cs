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
            if (currentUser.UserType != UserType.STUDENT) return Content("Только студенты могут проходить тесты.");
            ViewBag.Username = currentUser.Username;
            ViewBag.SessionId = Tools.getCurrentSessionId(_cache);

            Test currentTest = db.Tests.FirstOrDefault(x => x.Id == id);
            if (currentTest == null)
                return NotFound();

            if (db.Records.Any(x => x.UserId == currentUser.Id && x.TestId == id)) return Content("Вы уже проходили данный тест.");

            currentTest.Questions = db.Questions.Where(x => x.TestId == currentTest.Id).ToList();
            foreach(var item in currentTest.Questions)
            {
                item.Answers = db.Answers.Where(x => x.QuestionId == item.Id).ToList();
            }
            return View(currentTest);
        }

        public IActionResult MyTests(int userId, string search)
        {
            if (!Tools.isSessionActual(_cache)) return Redirect("../Auth");
            var currentUser = Tools.getCurrentUser(_cache);
            if (userId != currentUser.Id && currentUser.UserType != UserType.TEACHER) return Unauthorized();
            ViewBag.Username = currentUser.Username;
            ViewBag.UserId = currentUser.Id;

            var testOfCurrentUser = db.Tests.Where(x => x.CreatorId == currentUser.Id);

            if (search == null)
                return View(testOfCurrentUser);
            else
                return View(testOfCurrentUser.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string testName, string[] questions, int[] answersCount, string[] answers, int[] index)
        {
            if (!Tools.isSessionActual(_cache)) return Redirect("../Auth");
            var currentUser = Tools.getCurrentUser(_cache);
            if (currentUser.UserType != UserType.TEACHER) return Unauthorized();

            if (db.Tests.Any(x => x.Name == testName)) return Content("Тест с таким названием уже существует.");

            if (questions.Length<=0) return Content("Вы не можете создать тест без вопросов.");

            foreach (var item in answersCount)
                if (item <= 1) return Content("У вопроса должно быть минимум два ответа.");

            Test newTest = new Test();    
            newTest.Name = testName;
            newTest.CreatorId = currentUser.Id;

            int c = 0;

            for (int i = 0; i < questions.Length; i++)
            {
                Question question = new Question();
                question.Text = questions[i];
                question.CorrectAnswerId = index[i];
                for (int j = 0; j < answersCount[i]; j++)
                {
                    Answer answer = new Answer();
                    answer.Text = answers[c++];
                    question.Answers.Add(answer);
                }
                newTest.Questions.Add(question); 
            }

            db.Tests.Add(newTest);
            db.SaveChanges();

            return RedirectToAction("AllTests");
        }


        public IActionResult Create()
        {
            if (!Tools.isSessionActual(_cache)) return Redirect("../Auth");
            var currentUser = Tools.getCurrentUser(_cache);
            if (currentUser.UserType != UserType.TEACHER) return Unauthorized();
            ViewBag.Username = currentUser.Username;

            return View();
        }
    }
}