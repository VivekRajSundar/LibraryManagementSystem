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
        public (bool isUserAdded,string message) Register(string name, string email, string password, string confirmPassword)
        {
            //check if no fields are empty
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword)) 
                return (false, "One or more fields are empty");

            //check if it is a valid email
            if (!IsValidEmail(email)) return (false, "The Email format is wrong");

            //check if email already exists in db
            if (_userRepository.GetUser(email.ToLower()) is not null) return (false, "The user is already registered.");

            //check if both passwords are same
            if (!string.Equals(password, confirmPassword, StringComparison.Ordinal)) return (false, "Both passwords are not same.");

            //Generate Salt for this user
            string salt = GenerateSalt();

            //Hash the Password with the Salt generated
            string hashedPassword = HashPassword(password, salt);

            //Call repository method to add user in db
            bool isUserAdded = _userRepository.AddUser(new User(name, email.ToLower(), hashedPassword, salt));
            if (isUserAdded) return (true, "User Registered Successsfully.");
            return (false, "Something went wrong.");
        }

        public (bool isAuthenticated,string message) Login(string email, string password)
        {
            // check if email and password is valid or not
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) return (false, "Either Email or Password is empty");

            //Check if it is a valid email
            if(!IsValidEmail(email)) return (false, "The given email is invalid.");

            // call repository method to verify the login
            User? user = _userRepository.GetUser(email.ToLower());
            if (user is null) return (false, "The email is not registered.");

            //Verify the password
            string hashedPassword = HashPassword(password, user.Salt);

            if (user.Password == hashedPassword)
            {
                SessionManager.CurrentUser = user;
                return (true, "Authentication successfull.");
            }

            return (false, "The password is not matching.");
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

        #region Private methods
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
        #endregion
    }
}
