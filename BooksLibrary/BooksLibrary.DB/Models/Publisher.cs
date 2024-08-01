namespace BooksLibrary.DB
{
    public class Publisher
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual List<Book> Books { get; set; } = [];

        public override string? ToString()
        {
            return Name;
        }
    }
}
