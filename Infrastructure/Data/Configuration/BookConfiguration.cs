using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fingers10.EnterpriseArchitecture.Infrastructure.Data.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Book").HasKey(k => k.Id);

            builder.Property(p => p.Title)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasConversion(p => p.Value, p => Title.Create(p).Value);

            builder.Property(p => p.Description)
                   .HasMaxLength(1500)
                   .HasConversion(p => p.Value, p => Description.Create(p).Value);

            builder.HasOne(p => p.Author).WithMany(p => p.Books);
        }
    }
}
