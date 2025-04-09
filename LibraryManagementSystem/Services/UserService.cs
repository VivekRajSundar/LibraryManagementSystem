using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LibraryManagementSystem.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        public UserService()
        {
            _userRepository = new UserRepository();
        }
        public bool Register(string name, string email, string password, string confirmPassword)
        {
            //check if no fields are empty
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword)) return false;

            //check if it is a valid email
            if (!IsValidEmail(email)) return false;

            //check if email already exists in db
            if (_userRepository.GetUser(email.ToLower()) is not null) return false;

            //check if both passwords are same
            if (!string.Equals(password, confirmPassword, StringComparison.Ordinal)) return false;

            //Generate Salt for this user
            string salt = GenerateSalt();

            //Hash the Password with the Salt generated
            string hashedPassword = HashPassword(password, salt);

            //Call repository method to add user in db
            return _userRepository.AddUser(new User(name, email.ToLower(), hashedPassword, salt));
        }

        public bool Login(string email, string password)
        {
            // check if email and password is valid or not
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) return false;

            //Check if it is a valid email
            if(!IsValidEmail(email)) return false;

            // call repository method to verify the login
            User? user = _userRepository.GetUser(email.ToLower());
            if (user is null) return false;

            //Verify the password
            string hashedPassword = HashPassword(password, user.Salt);

            if (user.Password == hashedPassword) SessionManager.CurrentUser = user;

            return user.Password == hashedPassword;
        }

        public void Logout() => SessionManager.CurrentUser = null;
        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();            
        }        

        public bool IsUserBorrowedBooks(string email)
        {
            return _userRepository.IsUserBorrowedBook(email);
        }

        private static bool IsValidEmail(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailPattern);
        }

        private static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            RandomNumberGenerator.Fill(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        private static string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            byte[] saltedPassword = Encoding.UTF8.GetBytes(password + salt);
            byte[] hash = sha256.ComputeHash(saltedPassword);
            return Convert.ToBase64String(hash);
        }
    }
}
