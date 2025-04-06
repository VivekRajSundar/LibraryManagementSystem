using LibraryManagementSystem.Data;
using LibraryManagementSystem.Enums;
using LibraryManagementSystem.Services;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;

namespace LibraryManagementSystem
{
    internal class Program
    {
        private static UserService _userService = new UserService();
        private static BookService _bookService = new BookService();
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

            do
            {
                Console.WriteLine("Hi There, Before Entering into Library Please Verify yourself\n1.Register\n2.Login\n3.Exit");
                Console.Write("Enter your choice: ");
                int.TryParse(Console.ReadLine(), out int choice);
                switch (choice)
                {
                    case 1:
                        Register();
                        break;
                    case 2:
                        if (Login()) { ViewLibrary(); }
                        else Console.WriteLine("Authentication not successful.");
                        break;
                    case 3:
                        Console.WriteLine("Exiting from the LibraryManagement App");
                        canContinue = false; break;
                    default:
                        Console.WriteLine("The choice is not valid"); break;
                }
            } while (canContinue);
        }
        static void ViewLibrary()
        {
            Console.WriteLine($"Hi {SessionManager.CurrentUser.Name}, Welcome to Library Management System!");
            bool canContinue = true;
            do
            {
                Console.WriteLine("Menu: ");
                foreach (MemberActivity activity in Enum.GetValues(typeof(MemberActivity)))
                {
                    Console.WriteLine($"{(int)activity}. {activity}");
                }
                if (SessionManager.CurrentUser.Role.ToLower() == "admin")
                {
                    foreach (AdminActivity activity in Enum.GetValues(typeof(AdminActivity)))
                    {
                        Console.WriteLine($"{(int)activity}. {activity}");
                    }
                }
                Console.Write($"Choose your options: ");

                try
                {
                    _ = int.TryParse(Console.ReadLine(), out int option);
                    if (option > 100 && SessionManager.CurrentUser.Role.ToLower() != "admin")
                    {
                        Console.WriteLine("Invalid Choice");
                        continue;
                    }
                    switch (option) //need to rewrite the enum logic for separate roles.
                    {
                        case (int)AdminActivity.AddBook:
                            AddBook();
                            break;
                        case (int)AdminActivity.ViewAllUsers:
                            //View all Users
                            Console.WriteLine("Viewing All Users");
                            break;
                        case (int)MemberActivity.ViewBooks:
                            _bookService.ListBooks();
                            break;
                        case (int)MemberActivity.BorrowBook:
                            //Borrow Book
                            Console.WriteLine("Borrow Book");
                            BorrowBook();
                            break;
                        case (int)MemberActivity.ReturnBook:
                            //Return Book
                            Console.WriteLine("Return Book");
                            break;
                        case (int)MemberActivity.Logout:
                            Console.WriteLine($"See you next time, {SessionManager.CurrentUser.Name}!");
                            canContinue = false;
                            Logout();
                            break;
                        default:
                            Console.WriteLine("Invalid Choice"); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                    canContinue = false;
                }
            } while (canContinue);
        }
        static void Register()
        {
            Console.Write("Enter your Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter your Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();
            Console.Write("Confirm your password: ");
            string confirmPassword = Console.ReadLine();
            bool isUserAdded = _userService.Register(name, email, password, confirmPassword);
            if (isUserAdded) Console.WriteLine("User registered successfully.");
            else Console.WriteLine("Something went wrong, user not added"); //later change this to show exact error message.
        }
        static bool Login()
        {
            Console.Write("Enter you email: ");
            string email = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();
            return _userService.Login(email, password);
        }
        static void AddBook()
        {
            Console.WriteLine("Enter the ISBN: ");
            _ = int.TryParse(Console.ReadLine(), out int isbn);
            Console.WriteLine("Enter the Name of the Book: ");
            string bookName = Console.ReadLine();
            Console.WriteLine("Enter the Author Name: ");
            string authorName = Console.ReadLine();
            Console.WriteLine("Enter the number of copies that you are adding: ");
            _ = int.TryParse(Console.ReadLine(), out int copies);

            bool isBookAdded = _bookService.AddBook(isbn, bookName, authorName, copies);
            if (isBookAdded) Console.WriteLine("Book added successfully.");
            else Console.WriteLine("There is some issue in adding the books"); //later change this code to show exact error.
        }
        static void BorrowBook()
        {
            Console.Write("Enter the ISBN number of the Book: ");
            _ = int.TryParse(Console.ReadLine(), out int isbn);

            //Check if the user currently borrowed a book or not
            if (_userService.IsUserBorrowedBooks(SessionManager.CurrentUser.Email)) { Console.WriteLine("You already borrowed a book."); return; }

            bool isBookBorrowed = _bookService.BorrowBook(isbn);
            if (isBookBorrowed) Console.WriteLine("Book borrowed successfully.");
            else Console.WriteLine("Something went wrong. Book is not borrowed.");
        }
        static void Logout() => _userService.Logout();
    }
}
