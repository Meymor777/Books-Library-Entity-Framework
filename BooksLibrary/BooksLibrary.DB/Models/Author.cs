namespace BooksLibrary.DB
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Book> Books { get; set; } = [];

        public override string? ToString()
        {
            return Name;
        }
    }
}
