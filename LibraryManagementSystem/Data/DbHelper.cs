using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data
{
    public class DbHelper
    {
        private static string absolutePath = "C:\\Users\\vivek\\source\\repos\\LibraryManagementSystem\\LibraryManagementSystem\\library.db";
        private static string connectionString = $"Data Source ={absolutePath};version=3;";

        public static SQLiteConnection GetConnection() => new SQLiteConnection(connectionString);        
    }
}
