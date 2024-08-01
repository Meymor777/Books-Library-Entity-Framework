using Microsoft.EntityFrameworkCore;

namespace BooksLibrary.DB
{
    public class AuthorRepository
    {
        private readonly BooksLibraryDBContext _dbContext;
        public AuthorRepository(BooksLibraryDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<List<Author>> Get()
        {
            return await _dbContext
                .Authors
                .AsNoTracking()
                .Include(x => x.Books)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
        public async Task<Author?> Get(Guid id)
        {
            return await _dbContext
                .Authors
                .AsNoTracking()
                .Include(x => x.Books)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Author?> Get(string name)
        {
            return await _dbContext
                .Authors
                .AsNoTracking()
                .Include(x => x.Books)
                .FirstOrDefaultAsync(x => x.Name == name);
        }
        public async Task Add(Guid id, string name)
        {
            Author author = new Author
            {
                Id = id,
                Name = name,
            };

            await _dbContext.AddAsync(author);
            await _dbContext.SaveChangesAsync();
        }
        public async Task Update(Guid id, string name)
        {
            await _dbContext
                .Authors
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(x => x.Name, name));
        }
        public async Task Delete(Guid id)
        {
            await _dbContext
                .Authors
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }
        public async Task DeleteAll()
        {
            await _dbContext
                .Authors
                .ExecuteDeleteAsync();
        }
    }
}

