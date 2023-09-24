namespace UserLogin
{
    public class User
    {
        private string email;
        public string Email { get; set; }
        private string username;
        public string Username { get; set; }
        private string password;
        public string Password { get; set; }
        private int id;
        public int Id { get; set; }
        public User()
        {
            Email = "undefined";
            Username = "undefined";
            Password = "undefined";
            id = 0;
        }
    }
}