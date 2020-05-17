using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fingers10.EnterpriseArchitecture.Infrastructure.Data.Configuration
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Student").HasKey(k => k.Id);
            builder.Property(p => p.Id).HasColumnName("StudentID");
            builder.Property(p => p.Email)
                .HasConversion(p => p.Value, p => Email.Create(p).Value);
            builder.OwnsOne(p => p.Name, p =>
            {
                p.Property<long?>("NameSuffixID").HasColumnName("NameSuffixID");
                p.Property(pp => pp.First).HasColumnName("FirstName");
                p.Property(pp => pp.Last).HasColumnName("LastName");
                p.HasOne(pp => pp.Suffix).WithMany().HasForeignKey("NameSuffixID").IsRequired(false);
            });
            builder.HasOne(p => p.FavoriteCourse).WithMany();
            builder.HasMany(p => p.Enrollments).WithOne(p => p.Student)
                .OnDelete(DeleteBehavior.Cascade)
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
