using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services
{
    public class BookService
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
            
            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();
                string query = "Select * from Books";
                using var command = new SQLiteCommand(query, connection); 
                using var reader = command.ExecuteReader();

                while (reader.Read()) {
                    books.Add(new Book
                    (reader.GetInt32(1),
                      reader.GetString(2),
                      reader.GetString(3),
                      reader.GetInt32(4)
                    ));
                }
            }
            return books;
        }

        public void ListBooks()
        {
            List<Book> books = GetAllBooks();
            Console.WriteLine("List of Books");
            foreach (Book book in books) {
                Console.WriteLine($"{book.ISBN}\t{book.Name}\t{book.Author}\t{book.CopiesAvailable}");
            }

        }
    }
}
