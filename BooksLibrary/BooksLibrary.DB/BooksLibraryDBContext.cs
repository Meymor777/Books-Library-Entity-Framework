using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BooksLibrary.DB
{
    public class BooksLibraryDBContext : DbContext, IBooksLibraryDBContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        public BooksLibraryDBContext() { }
        public BooksLibraryDBContext(DbContextOptions<BooksLibraryDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new GenreConfiguration());
            modelBuilder.ApplyConfiguration(new PublisherConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);  
            IConfiguration config = builder.Build();
            string connectionString = config.GetSection("ConnectionString").GetSection("SqlServer").Value;

            optionsBuilder.UseSqlServer(connectionString);
        }
        public BooksLibraryDBContext GetBooksLibraryDBContext()
        {
            return new BooksLibraryDBContext(new DbContextOptionsBuilder<BooksLibraryDBContext>().Options);
        }
    }
}
