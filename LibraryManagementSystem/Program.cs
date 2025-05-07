using LibraryManagementSystem.Data;
using LibraryManagementSystem.Enums;
using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Services;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
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
            var entryRule = new Rule("App Start");
            entryRule.Style = Style.Parse("red");
            AnsiConsole.Write(entryRule);

            var exitrule = new Rule("Exiting from the LibraryManagement App");
            exitrule.Style = Style.Parse("red");
            do
            {                
                string choice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title("Choose a option to continue")
                            .AddChoices(["Login", "Register", "Exit"])
                            );
                switch (choice)
                {
                    case "Register":
                        Register();
                        break;
                    case "Login":
                        if (Login()) { ViewLibrary(); }
                        else OutputHelper.ErrorMsg("Authentication not successful.");
                        break;
                    case "Exit":
                        AnsiConsole.Write(exitrule);
                        canContinue = false; break;
                    default:
                        OutputHelper.ErrorMsg("The choice is not valid"); break;
                }
            } while (canContinue);
        }

        /// <summary>
        /// The Library Menu.
        /// </summary>
        static void ViewLibrary()
        {
            int totalEnumSize = Enum.GetNames(typeof(MemberActivity)).Length;
            bool isAdmin = SessionManager.CurrentUser.Role.ToLower() == "admin";
            if (isAdmin) totalEnumSize += Enum.GetNames(typeof(AdminActivity)).Length;
            string[] messages = new string[totalEnumSize];
            int count = 0;
            foreach (MemberActivity activity in Enum.GetValues(typeof(MemberActivity))) messages[count++] = activity.ToString();
            
            if (SessionManager.CurrentUser.Role.ToLower() == "admin")
            {
                foreach (AdminActivity activity in Enum.GetValues(typeof(AdminActivity))) messages[count++] = activity.ToString();                
            }
            
            bool canContinue = true;
            var loginRule = new Rule($"Hello [darkolivegreen2]{SessionManager.CurrentUser.Name}[/], WelCome to [orangered1]Library[/]");
            loginRule.Style = Style.Parse("tan");
            AnsiConsole.Write(loginRule);

            var logoutRule = new Rule($"See you next time, [darkolivegreen2]{SessionManager.CurrentUser.Name}[/]!");
            logoutRule.Style = Style.Parse("tan");            
            do
            {
                try
                {
                    var choice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title("Select what you wanna do")
                            .AddChoices(messages)
                        );

                    int option = -1;
                    if(Enum.TryParse<MemberActivity>(choice, out var result))  option = (int)result;
                    else if(Enum.TryParse<AdminActivity>(choice,out var result1))  option = (int)result1;
                                        
                    switch (option) 
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
                            AnsiConsole.Write(logoutRule);
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
            string name = AnsiConsole.Ask<string>("What's your name?");
            string email = AnsiConsole.Ask<string>("What's your email?");
            string password = AnsiConsole.Ask<string>("Enter your password:");
            string confirmPassword = AnsiConsole.Ask<string>("Confirm your password:");

            bool isUserAdded = _userService.Register(name, email, password, confirmPassword);
            if (isUserAdded) OutputHelper.SuccessMsg("User registered successfully.");
            else OutputHelper.ErrorMsg("Something went wrong, user not added"); //later change this to show exact error message.
        }
        static bool Login()
        {            
            string email = AnsiConsole.Ask<string>("What's your email?");
            string password = AnsiConsole.Prompt(new TextPrompt<string>("Enter your password: ").Secret());
            return _userService.Login(email, password);
        }
        static void AddBook()
        {
            int isbn = AnsiConsole.Ask<int>("Enter the ISBN: ");            
            string bookName = AnsiConsole.Ask<string>("Enter the Name of the Book: ");                                    
            string authorName = AnsiConsole.Ask<string>("Enter the Author Name: ");
            int copies = AnsiConsole.Ask<int>("Enter the number of copies that you are adding: ");            

            bool isBookAdded = _bookService.AddBook(isbn, bookName, authorName, copies);
            if (isBookAdded) OutputHelper.SuccessMsg("Book added successfully.");
            else OutputHelper.ErrorMsg("There is some issue in adding the books"); //later change this code to show exact error.
        }
        static void BorrowBook()
        {            
            //Check if the user currently borrowed a book or not
            if (_userService.IsUserBorrowedBooks(SessionManager.CurrentUser.Email)) { Console.WriteLine("You already borrowed a book."); return; }
            
            int isbn = AnsiConsole.Ask<int>("Enter the ISBN number of the Book: ");

            bool isBookBorrowed = _bookService.BorrowBook(isbn);
            if (isBookBorrowed) OutputHelper.SuccessMsg("You borrowed the Book successfully.");
            else OutputHelper.ErrorMsg("Something went wrong. Book is not borrowed.");
        }
        static void ReturnBook()
        {
            //check if the user borrowed any books
            if (!_userService.IsUserBorrowedBooks(SessionManager.CurrentUser.Email)) { Console.WriteLine("You don't borrowed any books that you can return"); return; }

            int isbn = AnsiConsole.Ask<int>("Enter the ISBN number of the Book that you want to return: ");

            bool isBookReturned = _bookService.ReturnBook(isbn);
            if (isBookReturned) OutputHelper.SuccessMsg("You returned the book sucessfully.");
            else OutputHelper.ErrorMsg("Book Return was unsuccessful. Kindly try again.");
        }
        static void Logout() => _userService.Logout();
    }
}
