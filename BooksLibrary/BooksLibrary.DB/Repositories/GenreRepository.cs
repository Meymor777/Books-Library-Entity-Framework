using Microsoft.EntityFrameworkCore;

namespace BooksLibrary.DB
{
    public class GenreRepository
    {
        private readonly BooksLibraryDBContext _dbContext;
        public GenreRepository(BooksLibraryDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<List<Genre>> Get()
        {
            return await _dbContext
                .Genres
                .AsNoTracking()
                .Include(x => x.Books)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
        public async Task<Genre?> Get(Guid id)
        {
            return await _dbContext
                .Genres
                .AsNoTracking()
                .Include(x => x.Books)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Genre?> Get(string name)
        {
            return await _dbContext
                .Genres
                .AsNoTracking()
                .Include(x => x.Books)
                .FirstOrDefaultAsync(x => x.Name == name);
        }
        public async Task Add(Guid id, string name)
        {
            Genre genre = new Genre
            {
                Id = id,
                Name = name,
            };

            await _dbContext.AddAsync(genre);
            await _dbContext.SaveChangesAsync();
        }
        public async Task Update(Guid id, string name)
        {
            await _dbContext
                .Genres
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(x => x.Name, name));
        }
        public async Task Delete(Guid id)
        {
            await _dbContext
                .Genres
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }
        public async Task DeleteAll()
        {
            await _dbContext
                .Genres
                .ExecuteDeleteAsync();
        }
    }
}

