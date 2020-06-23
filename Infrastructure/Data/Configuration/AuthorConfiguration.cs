using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fingers10.EnterpriseArchitecture.Infrastructure.Data.Configuration
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("Author").HasKey(k => k.Id);

            builder.OwnsOne(p => p.Name, p =>
            {
                p.Property(pp => pp.First).HasColumnName("FirstName").IsRequired().HasMaxLength(50);
                p.Property(pp => pp.Last).HasColumnName("LastName").IsRequired().HasMaxLength(50);
            });

            builder.Property(p => p.DateOfBirth)
                   .IsRequired()
                   .HasConversion(p => p.Value, p => BirthDate.Create(p).Value);

            builder.Property(p => p.DateOfDeath)
                   .HasConversion(p => p.Value, p => DeathDate.Create(p).Value);

            builder.Property(p => p.MainCategory)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasConversion(p => p.Value, p => MainCategory.Create(p).Value);

            builder.HasMany(p => p.Books).WithOne(p => p.Author)
                .OnDelete(DeleteBehavior.Cascade)
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
