using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooksLibrary.DB
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).HasMaxLength(200);

            builder
                .HasOne(x => x.Author)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.Genre)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.GenreId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.Publisher)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.PublisherId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
