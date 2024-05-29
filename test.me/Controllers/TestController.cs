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

        public IActionResult Index(int id) {
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

        [HttpPost]
        public IActionResult CheckTest()
        {
            var queryParams = HttpContext.Request.Query;
            var parameters = new Dictionary<string, string>();

            foreach (var param in queryParams)
            {
                parameters.Add(param.Key, param.Value);
            }

            return Content(string.Join(" ", parameters));
        }

    }
}