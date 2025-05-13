using LibraryManagementSystem.Models;
using System.Data.SQLite;

namespace LibraryManagementSystem.Data
{
    public class BookRepository
    {
        public bool AddBook(Book book)
        {
            try
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
            catch (Exception ex) {
                return false;
            }
            return true;
        }

        /// <summary>        
        /// One User can borrow one book only at a time.
        /// </summary>
        /// <param name="isbn"></param>
        /// <param name="email"></param>
        public void BorrowBook(int isbn, string email)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            //update BorrowedBooks table with the email and isbn.
            string query = "INSERT INTO BorrowedBooks(User_Email, Books_Isbn, ReturnedDate) VALUES(@Email, @Isbn, @ReturnedDate);";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Isbn", isbn);
                command.Parameters.AddWithValue("@ReturnedDate", DBNull.Value);
                command.ExecuteNonQuery();
            }

            //Decrease the copies count from the Books table.
            query = "UPDATE Books SET CopiesAvailable = CopiesAvailable - 1 WHERE ISBN = @isbn;";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@isbn", isbn);
                command.ExecuteNonQuery();
            }
        }
        public void ReturnBook(int isbn, string email)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            //update BorrowedBooks with current date value for the respective Id
            string query = "UPDATE BorrowedBooks SET ReturnedDate = datetime('now') WHERE Books_isbn = @isbn and User_Email = @email;";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@isbn", isbn);
                command.Parameters.AddWithValue("@email", email);
                command.ExecuteNonQuery();
            }

            //Increase the count of books table where the isbn is present
            query = "UPDATE Books SET CopiesAvailable = CopiesAvailable + 1 WHERE isbn = @isbn;";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@isbn", isbn);
                command.ExecuteNonQuery();
            }
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

        public bool IsUserBorrowedBook(int isbn, string email)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            string query = "Select * from BorrowedBooks where Books_isbn = @isbn and User_email = @email";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@isbn", isbn);
            command.Parameters.AddWithValue("@email", email);
            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;           
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
            while (reader.Read())
            {
                book = new Book(
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetInt32(4)
                );
            }
            return book;
        }
    }
}
