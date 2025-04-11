using LibraryManagementSystem.Data;
using LibraryManagementSystem.Enums;
using LibraryManagementSystem.Helpers;
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
                //Console.WriteLine("\n1.Register\n2.Login\n3.Exit");
                OutputHelper.ShowMenu("Hi There, Before Entering into Library Please Verify yourself", ["Register", "Login", "Exit"]);
                int.TryParse(Console.ReadLine(), out int choice);
                switch (choice)
                {
                    case 1:
                        Register();
                        break;
                    case 2:
                        if (Login()) { ViewLibrary(); }
                        else OutputHelper.ErrorMsg("Authentication not successful.");
                        break;
                    case 3:
                        Console.WriteLine("Exiting from the LibraryManagement App");
                        canContinue = false; break;
                    default:
                        OutputHelper.ErrorMsg("The choice is not valid"); break;
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
                        OutputHelper.ErrorMsg("Invalid Choice");
                        continue;
                    }
                    switch (option) //need to rewrite the enum logic for separate roles.
                    {
                        case (int)AdminActivity.AddBook:
                            AddBook();
                            break;
                        case (int)AdminActivity.ViewAllUsers:
                            OutputHelper.ListUsers(_userService.GetAllUsers());
                            break;
                        case (int)MemberActivity.ViewBooks:
                            OutputHelper.ListBooks(_bookService.GetAllBooks());
                            break;
                        case (int)MemberActivity.BorrowBook:
                            BorrowBook();
                            break;
                        case (int)MemberActivity.ReturnBook:
                            ReturnBook();
                            break;
                        case (int)MemberActivity.Logout:
                            Console.WriteLine($"See you next time, {SessionManager.CurrentUser.Name}!");
                            canContinue = false;
                            Logout();
                            break;
                        default:
                            OutputHelper.ErrorMsg("Invalid Choice"); break;
                    }
                }
                catch (Exception ex)
                {
                    OutputHelper.ErrorMsg(ex.Message.ToString());
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
            if (isUserAdded) OutputHelper.SuccessMsg("User registered successfully.");
            else OutputHelper.ErrorMsg("Something went wrong, user not added"); //later change this to show exact error message.
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
            Console.Write("Enter the ISBN: ");
            _ = int.TryParse(Console.ReadLine(), out int isbn);
            Console.Write("Enter the Name of the Book: ");
            string bookName = Console.ReadLine();
            Console.Write("Enter the Author Name: ");
            string authorName = Console.ReadLine();
            Console.Write("Enter the number of copies that you are adding: ");
            _ = int.TryParse(Console.ReadLine(), out int copies);

            bool isBookAdded = _bookService.AddBook(isbn, bookName, authorName, copies);
            if (isBookAdded) OutputHelper.SuccessMsg("Book added successfully.");
            else OutputHelper.ErrorMsg("There is some issue in adding the books"); //later change this code to show exact error.
        }
        static void BorrowBook()
        {            
            //Check if the user currently borrowed a book or not
            if (_userService.IsUserBorrowedBooks(SessionManager.CurrentUser.Email)) { Console.WriteLine("You already borrowed a book."); return; }

            Console.Write("Enter the ISBN number of the Book: ");
            _ = int.TryParse(Console.ReadLine(), out int isbn);

            bool isBookBorrowed = _bookService.BorrowBook(isbn);
            if (isBookBorrowed) OutputHelper.SuccessMsg("You borrowed the Book successfully.");
            else OutputHelper.ErrorMsg("Something went wrong. Book is not borrowed.");
        }
        static void ReturnBook()
        {
            //check if the user borrowed any books
            if (!_userService.IsUserBorrowedBooks(SessionManager.CurrentUser.Email)) { Console.WriteLine("You don't borrowed any books that you can return"); return; }

            Console.Write("Enter the ISBN number of the Book that you want to return: ");
            _ = int.TryParse(Console.ReadLine(), out int isbn);

            bool isBookReturned = _bookService.ReturnBook(isbn);
            if (isBookReturned) OutputHelper.SuccessMsg("You returned the book sucessfully.");
            else OutputHelper.ErrorMsg("Book Return was unsuccessful. Kindly try again.");
        }
        static void Logout() => _userService.Logout();
    }
}
