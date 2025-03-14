using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    public class Library
    {
        private List<Book> books = new List<Book>();
        public void AddBooks(Book book) => books.Add(book);
        public void ListBooks()
        {
            Console.WriteLine($"List of Books:");
            int count = 1;
            foreach (Book book in books)
            {
                Console.WriteLine($"{count++} {book.Name} {book.Author}");
            }
        }
    }
}
