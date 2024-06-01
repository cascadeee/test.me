using System.ComponentModel.DataAnnotations.Schema;

namespace testme.Models
{
    public class Session
    {   
        public int Id { get; private set; }
        public int UserId { get; private set; }
        [ForeignKey("UserId")]
        public User User { get; private set; }
        public bool isActual { get; set; }
        public Guid SessionID { get; private set; }
        public DateTime sessionTime { get; private set; }
        
        public Session() { }

        public Session(int userId) { 
            UserId = userId;
            isActual = true;
            SessionID = Guid.NewGuid();
            sessionTime = DateTime.Now;
        }
    }
}
