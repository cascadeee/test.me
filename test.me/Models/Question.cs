using System.ComponentModel.DataAnnotations.Schema;

namespace testme.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public int TestId { get; set; }
        [ForeignKey("TestId")]
        public Test Test { get; set; }

        public int CorrectAnswerId { get; set; }

        public ICollection<Answer> Answers { get; set; }
        public Question()
        {
            Answers = new List<Answer>();
        }
    }
}
