using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using testme.Models;

namespace testme
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationContext db;

        public HomeController(IMemoryCache cache)
        {
            _cache = cache;
            db = new ApplicationContext();
        }
        public IActionResult Index(string search)
        {
            if (!Tools.isSessionActual(_cache)) return Redirect("Auth");
            var currentUser = Tools.getCurrentUser(_cache);

            ViewBag.Username = currentUser.Username;

            return View();
        }

        public IActionResult Detail(int id) {
            Test? currentTest = db.Tests.FirstOrDefault(x => x.Id == id);
            if (currentTest == null)
                return NotFound();

            currentTest.Questions = db.Questions.Where(x => x.TestId == currentTest.Id).ToList();
            foreach(var item in currentTest.Questions)
            {
                item.Answers = db.Answers.Where(x => x.QuestionId == item.Id).ToList();
            }
            return View(currentTest);
        }

        //private void createTest()
        //{
        //    ///////////////////////////////////////////////////////

        //    Answer a1 = new Answer();
        //    a1.Text = "Артем";
        //    Answer a2 = new Answer();
        //    a2.Text = "Егор";
        //    Answer a3 = new Answer();
        //    a3.Text = "Влад";
        //    Answer a4 = new Answer();
        //    a4.Text = "Женя";

        //    Question question1 = new Question();

        //    question1.Text = "Как зовут меня?";
        //    question1.Answers.Add(a1);
        //    question1.Answers.Add(a2);
        //    question1.Answers.Add(a3);
        //    question1.Answers.Add(a4);

        //    question1.CorrectAnswerId = 0;

        //    ///////////////////////////////////////////////////////

        //    Answer a5 = new Answer();
        //    a5.Text = "Гиталев";
        //    Answer a6 = new Answer();
        //    a6.Text = "Бычковский";
        //    Answer a7 = new Answer();
        //    a7.Text = "Хамков";
        //    Answer a8 = new Answer();
        //    a8.Text = "Бова";

        //    Question question2 = new Question();

        //    question2.Text = "Какая у меня фамилия?";
        //    question2.Answers.Add(a5);
        //    question2.Answers.Add(a6);
        //    question2.Answers.Add(a7);
        //    question2.Answers.Add(a8);

        //    question2.CorrectAnswerId = 0;

        //    ///////////////////////////////////////////////////////

        //    Test test1 = new Test();
        //    test1.Questions.Add(question1);
        //    test1.Questions.Add(question2);
        //    test1.Name = "Just test";

        //    ///////////////////////////////////////////////////////
            
        //    db.Tests.Add(test1);
        //    db.SaveChanges();
        //}
    }
}