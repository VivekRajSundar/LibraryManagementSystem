using LibraryManagementSystem.Models;
using System.Data.SQLite;

namespace LibraryManagementSystem.Data
{
    public class BookRepository
    {
        public void AddBook(Book book)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            string query = "INSERT INTO Books(ISBN, Name, Author, CopiesAvailable) VALUES(@ISBN, @Name, @Author, @CopiesAvailable)";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@ISBN", book.ISBN);
            command.Parameters.AddWithValue("@Name", book.Name);
            command.Parameters.AddWithValue("@Author", book.Author);
            command.Parameters.AddWithValue("@CopiesAvailable", book.CopiesAvailable);
            command.ExecuteNonQuery();
        }

        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();

            using var connection = DbHelper.GetConnection();
            connection.Open();
            string query = "Select * from Books";
            using var command = new SQLiteCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                books.Add(new Book
                (reader.GetInt32(1),
                  reader.GetString(2),
                  reader.GetString(3),
                  reader.GetInt32(4)
                ));
            }

            return books;
        }

        public Book? GetBook(int isbn)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            string query = "Select * from Books where isbn = @isbn";
            using var command = new SQLiteCommand (query, connection);
            command.Parameters.AddWithValue("@isbn", isbn);
            using var reader = command.ExecuteReader();
            Book? book = null;
            while (reader.Read()) book = new Book(reader.GetInt32(1),
                  reader.GetString(2),
                  reader.GetString(3),
                  reader.GetInt32(4)
                );
            return book;
        }
    }
}
