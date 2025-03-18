using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BookService _bookService = new BookService();
            Console.WriteLine($"Welcome to Library Management System!");
            bool canContinue = true;
            do
            {
                Console.WriteLine($"Choose your options: \n1. Add a new Book\n2. List All Books\n3. Exit");
                try
                {
                    int.TryParse(Console.ReadLine(), out int option);
                    if (option == 1)
                    {
                        Console.WriteLine("Enter the ISBN: ");
                        int.TryParse(Console.ReadLine(), out int isbn);
                        Console.WriteLine("Enter the Name of the Book: ");
                        string bookName = Console.ReadLine();
                        Console.WriteLine("Enter the Author Name: ");
                        string authorName = Console.ReadLine();
                        Console.WriteLine("Enter the number of copies that you are adding: ");
                        int.TryParse(Console.ReadLine(), out int copies);

                        _bookService.AddBook(new Book(isbn, bookName, authorName, copies));

                    }
                    else if (option == 2)
                    {
                        _bookService.ListBooks();
                    }
                    else if (option == 3)
                    {
                        canContinue = false;
                        Console.WriteLine("Bye bye");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            } while (canContinue);
        }
    }
}
