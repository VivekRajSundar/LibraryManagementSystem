using System.Data.SQLite;

namespace LibraryManagementSystem.Data
{
    public class DbHelper
    {
        private static string absolutePath = "C:\\Users\\vivek\\source\\repos\\LibraryManagementSystem\\LibraryManagementSystem\\library.db";
        private static string connectionString = $"Data Source ={absolutePath};version=3;";

        public static SQLiteConnection GetConnection() => new SQLiteConnection(connectionString);        

        public static void InitializeDB()
        {
            using var connection = GetConnection();
            connection.Open();

            //Create Users table if not exists
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                                CREATE TABLE IF NOT EXISTS Users(
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Name TEXT NOT NULL,
                                Email TEXT NOT NULL UNIQUE,
                                Password TEXT NOT NULL,
                                Salt TEXT  NOT NULL)
                                ";
            cmd.ExecuteNonQuery();
        }
    }
}
