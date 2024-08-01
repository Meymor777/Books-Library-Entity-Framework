namespace BooksLibrary.DB
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Pages { get; set; } = 0;
        public Guid GenreId { get; set; }
        public Genre? Genre { get; set; }
        public Guid AuthorId { get; set; }
        public Author? Author { get; set; }
        public Guid PublisherId { get; set; }
        public Publisher? Publisher { get; set; }
        public DateTime ReleaseDate { get; set; } = new DateTime();

        public override string? ToString()
        {
            return $"[{Title.PadRight(50)}]" +
                $"[{Pages.ToString().PadRight(4)}]" +
                $"[{Genre.ToString().PadRight(30)}]" +
                $"[{Author.ToString().PadRight(30)}]" +
                $"[{Publisher.ToString().PadRight(35)}]" +
                $"[{new DateOnly(ReleaseDate.Year, ReleaseDate.Month, ReleaseDate.Day)}]";
        }
    }
}
