using System.ComponentModel.DataAnnotations.Schema;

namespace testme.Models
{
    public class Session
    {   
        public int Id { get; private set; }

        public int UserId { get; private set; }
        [ForeignKey("UserId")]
        public User User { get; private set; }

        public Guid SessionID { get; private set; }
        public DateTime sessionTime { get; private set; }
        
        public Session() { }

        public Session(User user) { 
            User = user;
            SessionID = Guid.NewGuid();
            sessionTime = DateTime.Now;
        }
    }
}
