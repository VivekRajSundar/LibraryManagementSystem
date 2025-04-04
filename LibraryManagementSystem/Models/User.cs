namespace LibraryManagementSystem.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Role { get; set; }

        public User(string name, string email, string password, string salt, string role) : this(name, email, password, salt)
        {            
            Role = role;
        }

        public User(string name, string email, string password, string salt)
        {
            Name = name;
            Email = email;
            Password = password;
            Salt = salt;
        }
    }
}
