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
        public (bool isBookAdded, string message) AddBook(int isbn, string bookName, string author, int copies)
        {
            //check if all the field is not null or empty
            if (isbn < 0 || string.IsNullOrEmpty(bookName) || string.IsNullOrEmpty(author) || copies < 1) return (false, "");

            if(_bookRepository.GetBook(isbn) is not null) return (false, "The given book is already added.");

            bool isBookAdded = _bookRepository.AddBook(new Book(isbn, bookName, author, copies));
            if (isBookAdded) return (true, "Book added successfully");
            return (false, "Something went wrong");
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
        public bool ReturnBook(int isbn)
        {
            //Check for invalid parameters
            if (isbn < 0 || SessionManager.CurrentUser is null) return false;

            if(!_bookRepository.IsUserBorrowedBook(isbn, SessionManager.CurrentUser.Email)) return false;

            _bookRepository.ReturnBook(isbn, SessionManager.CurrentUser.Email);
            return true;
        }
        public List<Book> GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }                
    }
}
