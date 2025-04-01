using LibraryManagementSystem.Data;
using LibraryManagementSystem.Services;
using Microsoft.Extensions.Configuration;

namespace LibraryManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Load configuration from appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Set base path
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Load JSON file
                .Build();

            string connectionString = config.GetConnectionString("LibraryDb");
            DbHelper.InitializeDB(connectionString);
            bool isAuthenticated = false, canContinue = true;
            UserService _userService = new UserService();
            
            do
            {
                Console.WriteLine("Hi There, Before Entering into Library Please Verify yourself\n1.Register\n2.Login\n3.Exit");
                Console.Write("Enter your choice: ");
                int.TryParse(Console.ReadLine(), out int choice);
                string email = string.Empty, name = string.Empty;
                string password = string.Empty, confirmPassword = string.Empty;
                switch (choice)
                {
                    case 1:
                        Console.Write("Enter your Name: ");
                        name = Console.ReadLine();
                        Console.Write("Enter your Email: ");
                        email = Console.ReadLine();
                        Console.Write("Enter your password: ");
                        password = Console.ReadLine();
                        Console.Write("Confirm your password: ");
                        confirmPassword = Console.ReadLine();
                        bool isUserAdded = _userService.AddUser(name, email, password, confirmPassword);
                        if (isUserAdded) Console.WriteLine("User registered successfully.");
                        else Console.WriteLine("Something went wrong, user not added"); //later change this to show exact error message.
                        break;
                    case 2:
                        Console.Write("Enter you email: ");
                        email = Console.ReadLine();
                        Console.Write("Enter your password: ");
                        password= Console.ReadLine();
                        isAuthenticated = _userService.VerifyUser(email, password);
                        canContinue = !isAuthenticated;
                        if (!isAuthenticated) Console.WriteLine("Authentication not successful.");
                        break;
                    case 3:
                        Console.WriteLine("See you next time!"); 
                        canContinue = false; break;
                    default:
                        Console.WriteLine("The choice is not valid"); break;
                }
            } while (canContinue);

            if (isAuthenticated)
            {
                // Access Library
                LibraryMenu();
            }
        }

        static void LibraryMenu()
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
                        _ = int.TryParse(Console.ReadLine(), out int isbn);
                        Console.WriteLine("Enter the Name of the Book: ");
                        string bookName = Console.ReadLine();
                        Console.WriteLine("Enter the Author Name: ");
                        string authorName = Console.ReadLine();
                        Console.WriteLine("Enter the number of copies that you are adding: ");
                        _ = int.TryParse(Console.ReadLine(), out int copies);

                        _bookService.AddBook(isbn, bookName, authorName, copies);

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
                    canContinue = false;
                }
            } while (canContinue);
        }
    }
}
