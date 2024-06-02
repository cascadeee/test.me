using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using testme.Models;

namespace testme.Controllers
{
    public class RecordController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationContext db;

        public RecordController(IMemoryCache cache)
        {
            _cache = cache;
            db = new ApplicationContext();
        }

        public IActionResult MyRecords(int userId)
        {
            if (!Tools.isSessionActual(_cache)) return Redirect("../Auth");
            var currentUser = Tools.getCurrentUser(_cache);
            if (userId != currentUser.Id && currentUser.UserType != UserType.STUDENT) return Unauthorized();

            ViewBag.Username = currentUser.Username;

            var records = db.Records.Where(x => x.UserId == currentUser.Id).ToList();

            foreach (var record in records)
            {
                record.Test = db.Tests.FirstOrDefault(x=>x.Id == record.TestId);
            }

            return View(records);
        }

        [HttpPost]
        public IActionResult Create(int testId, string sessionId, Dictionary<int, int> answers)
        {
            if (!Tools.isSessionActual(_cache)) return Redirect("../Auth");
            var currentUser = Tools.getCurrentUser(_cache);
            if (currentUser.UserType != UserType.STUDENT && sessionId != Tools.getCurrentSessionId(_cache).ToString()) return Unauthorized();

            ViewBag.Username = currentUser.Username;

            Record newRecord = new Record();    
            newRecord.UserId = currentUser.Id;  
            newRecord.TestId = testId;

            int correctAnswers = 0;

            Test currentTest = db.Tests.FirstOrDefault(x => x.Id == testId);
            if (currentTest == null) return NotFound();

            currentTest.Questions = db.Questions.Where(x=>x.TestId == currentTest.Id).ToList();

            foreach (var question in currentTest.Questions)
            {
                if (question.CorrectAnswerId == answers[question.Id]) correctAnswers++; 
            }
            newRecord.Result = correctAnswers + " / " + currentTest.Questions.Count().ToString();
            db.Records.Add(newRecord);
            db.SaveChanges();

            return RedirectToAction("MyRecords");
        }
    }
}
