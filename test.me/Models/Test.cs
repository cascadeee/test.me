using System.ComponentModel.DataAnnotations.Schema;

namespace testme.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public User Creator { get; set; }
        public ICollection<Question> Questions { get; set; }
        public Test()
        {
            Questions = new List<Question>();
        }
    }
}
