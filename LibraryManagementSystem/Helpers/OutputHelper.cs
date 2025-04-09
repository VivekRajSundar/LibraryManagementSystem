using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Helpers
{
    public class OutputHelper
    {
        public static void ListBooks(List<Book> books)
        {
            Console.WriteLine("List of Books");
            foreach (Book book in books)
            {
                Console.WriteLine($"{book.ISBN}\t{book.Name}\t{book.Author}\t{book.CopiesAvailable}");
            }
        }

        public static void ListUsers(List<User> users) 
        {
            Console.WriteLine("List of Users: ");
            foreach (User user in users)
            {
                Console.WriteLine($"{user.Name}\t{user.Email}\t{user.Role}");
            }
        }
    }
}
