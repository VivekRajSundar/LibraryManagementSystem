using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Helpers
{
    public class OutputHelper
    {
        public static void ListBooks(List<Book> books)
        {
            BoxIt("List of Books", ConsoleColor.Cyan);
            foreach (Book book in books)
            {
                Console.WriteLine($"{book.ISBN}\t{book.Name}\t{book.Author}\t{book.CopiesAvailable}");
            }
        }

        public static void ListUsers(List<User> users) 
        {
            BoxIt("List of Users", ConsoleColor.Cyan);
            foreach (User user in users)
            {
                Console.WriteLine($"{user.Name}\t{user.Email}\t{user.Role}");
            }
        }

        public static void ShowMenu(string title, string[] options, ConsoleColor color)
        {
            BoxIt(title, ConsoleColor.White);
            int count = 1;
            foreach (string option in options) ColorIt($"{count++}. {option}", ConsoleColor.Gray);
            Line(title.Length + 10, '-', ConsoleColor.DarkGray);
            Console.ForegroundColor = color;
            Console.Write("Enter your choice: ");
            Console.ResetColor();
        }

        public static void ErrorMsg(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        public static void SuccessMsg(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        public static void WarningMsg(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        /// <summary>
        /// Make a box of dash(-) around the message.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="color"></param>
        private static void BoxIt(string msg, ConsoleColor color)
        {
            int msgLength = msg.Length;
            Line(msgLength + 10, '-', ConsoleColor.DarkGray);
            ColorIt($"{new string(' ', 5)}{msg}{new string(' ', 5)}", color);
            Line(msgLength + 10, '-', ConsoleColor.DarkGray);
        }

        private static void Line(int count, char character = '\0', ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(new string(character, count));
            Console.ResetColor();
        }
        private static void ColorIt(string msg, ConsoleColor color, ConsoleColor bgColor = ConsoleColor.Black) {
            Console.ForegroundColor= color;
            Console.BackgroundColor = bgColor;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
