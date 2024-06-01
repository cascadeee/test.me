namespace testme.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string? Username { get; private set; }
        public string? PasswordHash { get; private set; }
        public UserType UserType { get; private set; }
        public ICollection<Session> Sessions { get; set; }
        public User()
        {
            Sessions = new List<Session>();
        }
        public User(string username, string password, UserType userType)
        {
            Username = username;
            PasswordHash = MD5Hash.hashPassword(password);
            UserType = userType;
        }

        public static bool isUsernameExists(string username)
        {
            using (ApplicationContext db = new ApplicationContext())
                return db.Users.Any(x => x.Username == username);
        }

        public static User? getUser(string username, string password)
        {
            using (ApplicationContext db = new ApplicationContext())
                return db.Users.FirstOrDefault(x => x.Username == username && x.PasswordHash == MD5Hash.hashPassword(password));
        }

    }
}
