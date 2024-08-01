using Microsoft.EntityFrameworkCore;

namespace BooksLibrary.DB
{
    public class PublisherRepository
    {
        private readonly BooksLibraryDBContext _dbContext;
        public PublisherRepository(BooksLibraryDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<List<Publisher>> Get()
        {
            return await _dbContext
                .Publishers
                .AsNoTracking()
                .Include(x => x.Books)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
        public async Task<Publisher?> Get(Guid id)
        {
            return await _dbContext
                .Publishers
                .AsNoTracking()
                .Include(x => x.Books)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Publisher?> Get(string name)
        {
            return await _dbContext
                .Publishers
                .AsNoTracking()
                .Include(x => x.Books)
                .FirstOrDefaultAsync(x => x.Name == name);
        }
        public async Task Add(Guid id, string name)
        {
            Publisher publisher = new Publisher
            {
                Id = id,
                Name = name,
            };

            await _dbContext.AddAsync(publisher);
            await _dbContext.SaveChangesAsync();
        }
        public async Task Update(Guid id, string name)
        {
            await _dbContext
                .Publishers
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(x => x.Name, name));
        }
        public async Task Delete(Guid id)
        {
            await _dbContext
                .Publishers
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }
        public async Task DeleteAll()
        {
            await _dbContext
                .Publishers
                .ExecuteDeleteAsync();
        }
    }
}

