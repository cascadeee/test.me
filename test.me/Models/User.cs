namespace testme.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string? Username { get; private set; }
        public string? PasswordHash { get; private set; }
        public UserType UserType { get; private set; }

        public User() { }
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
    }
}
