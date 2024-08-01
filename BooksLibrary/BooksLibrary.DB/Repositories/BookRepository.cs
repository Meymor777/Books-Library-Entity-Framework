using Microsoft.EntityFrameworkCore;

namespace BooksLibrary.DB
{
    public class BookRepository
    {
        private readonly BooksLibraryDBContext _dbContext;
        public BookRepository(BooksLibraryDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<List<Book>> Get()
        {
            return await _dbContext
                .Books
                .AsNoTracking()
                .Include(x => x.Author)
                .Include(x => x.Genre)
                .Include(x => x.Publisher)
                .OrderBy(x => x.Title)
                .ToListAsync();
        }
        public async Task<Book?> Get(Guid id)
        {
            return await _dbContext
                .Books
                .AsNoTracking()
                .Include(x => x.Author)
                .Include(x => x.Genre)
                .Include(x => x.Publisher)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Book?> Get(string title)
        {
            return await _dbContext
                .Books
                .AsNoTracking()
                .Include(x => x.Author)
                .Include(x => x.Genre)
                .Include(x => x.Publisher)
                .FirstOrDefaultAsync(x => x.Title == title);
        }
        public async Task Add(Guid id, string title, int pages, Genre genre, Author author, Publisher publisher, DateTime releaseDate)
        {
            Book book = new Book
            {
                Id = id,
                Title = title,
                Pages = pages,
                GenreId = genre.Id,
                AuthorId = author.Id,
                PublisherId = publisher.Id,
                ReleaseDate = releaseDate
            };

            await _dbContext.AddAsync(book);
            await _dbContext.SaveChangesAsync();
        }
        public async Task Update(Guid id, string title, int pages, Genre genre, Author author, Publisher publisher, DateTime releaseDate)
        {
            await _dbContext
                .Books
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(x => x.Title, title)
                .SetProperty(x => x.Pages, pages)
                .SetProperty(x => x.GenreId, genre.Id)
                .SetProperty(x => x.AuthorId, author.Id)
                .SetProperty(x => x.PublisherId, publisher.Id)
                .SetProperty(x => x.ReleaseDate, releaseDate));
        }
        public async Task Delete(Guid id)
        {
            await _dbContext
                .Books
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }
        public async Task DeleteAll()
        {
            await _dbContext
                .Books
                .ExecuteDeleteAsync();
        }
    }
}

