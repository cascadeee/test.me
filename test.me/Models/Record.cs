using System.ComponentModel.DataAnnotations.Schema;

namespace testme.Models
{
    public class Record
    {
        public int Id { get; set; }
        public DateTime RecordTime { get; private set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int TestId { get; set; }
        [ForeignKey("TestId")]
        public Test Test { get; set; }
        public string Result { get; set; }

        public Record()
        {
            RecordTime = DateTime.Now;
        }
    }
}
