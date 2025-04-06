using System.Data.SQLite;

namespace LibraryManagementSystem.Data
{
    public class DbHelper
    {
        private static string _connectionString;        
        public static SQLiteConnection GetConnection() => new SQLiteConnection(_connectionString);        

        public static void InitializeDB(string connectionString)
        {
            _connectionString = connectionString;
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
                                Salt TEXT  NOT NULL,
                                Role TEXT DEFAULT ""Member"" NOT NULL)
                                ";
            cmd.ExecuteNonQuery();

            //Create Books table if not exists
            cmd.CommandText = @"
                                CREATE TABLE IF NOT EXISTS Books(
                                Id Integer Primary Key AutoIncrement,
                                ISBN Integer Not Null UNIQUE,
                                Name Text Not Null,
                                Author Text Not Null,
                                CopiesAvailable Integer)
                                ";
            cmd.ExecuteNonQuery();

            //Set foreign keys on
            cmd.CommandText = @"PRAGMA foreign_keys = ON";
            cmd.ExecuteNonQuery();

            //Create Borrow Table
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS BorrowedBooks(
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                User_Email TEXT,
                                Books_Isbn Integer,
                                ReturnedDate TEXT,
                                FOREIGN KEY (User_Email) REFERENCES Users (Email) ON DELETE CASCADE,
                                FOREIGN KEY (Books_Isbn) REFERENCES Books (ISBN) ON DELETE CASCADE)
                                ";
            cmd.ExecuteNonQuery();
        }
    }
}
