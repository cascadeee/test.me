using System.ComponentModel.DataAnnotations.Schema;

namespace testme.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
    }
}
