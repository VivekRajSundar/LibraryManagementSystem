using LibraryManagementSystem.Models;
using System.Data.SQLite;


namespace LibraryManagementSystem.Data
{
    public class UserRepository
    {
        public bool AddUser(User user)
        {
            try
            {
                using var connection = DbHelper.GetConnection();
                connection.Open();
                string query = "INSERT INTO Users(Name, Email, Password, Salt) VALUES(@Name, @Email, @Password, @Salt)";
                using var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Salt", user.Salt);
                command.ExecuteNonQuery();
            }
            catch (Exception ex) { 
                return false;
            }
            return true;
        }

        public User? GetUser(string email)
        {
            User? user = null;
            using var connection = DbHelper.GetConnection();
            connection.Open();
            string query = "SELECT * FROM Users WHERE Email = @Email";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email);
            using var reader = command.ExecuteReader();
            while (reader.Read()) user = new User(reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
            return user;
        }
    }
}
