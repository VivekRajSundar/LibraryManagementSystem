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
        public bool AddBook(int isbn, string bookName, string author, int copies)
        {
            //check if all the field is not null or empty
            if (isbn < 0 || string.IsNullOrEmpty(bookName) || string.IsNullOrEmpty(author) || copies < 1) return false;

            if(_bookRepository.GetBook(isbn) is not null) return false;

            _bookRepository.AddBook(new Book(isbn, bookName, author, copies));
            return true;
        }

        public bool BorrowBook(int isbn)
        {            
            //Check for invalid parameters
            if(isbn < 0 || SessionManager.CurrentUser is null) return false;            

            //Check if the book exists in our library
            Book? book = _bookRepository.GetBook(isbn);
            if (book is null || book.CopiesAvailable < 1) return false;

            _bookRepository.BorrowBook(isbn, SessionManager.CurrentUser.Email);
            return true;
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
