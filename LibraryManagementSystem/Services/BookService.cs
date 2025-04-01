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
        private readonly BookRepository _bookRepository;
        public BookService() {
            _bookRepository = new BookRepository();
        }
        public void AddBook(int isbn, string bookName, string author, int copies)
        {
            _bookRepository.AddBook(new Book(isbn, bookName, author, copies));
        }
        public List<Book> GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
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
