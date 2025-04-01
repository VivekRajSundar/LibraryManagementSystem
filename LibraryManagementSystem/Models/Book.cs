namespace LibraryManagementSystem.Models
{
    public class Book
    {
        public int ISBN { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int CopiesAvailable { get; set; }

        public Book(int ISBN, string Name, string Author, int copies)
        {
            this.ISBN = ISBN;
            this.Name = Name;
            this.Author = Author;
            CopiesAvailable = copies;
        }
    }
}
