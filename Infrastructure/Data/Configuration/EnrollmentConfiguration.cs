using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fingers10.EnterpriseArchitecture.Infrastructure.Data.Configuration
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.ToTable("Enrollment").HasKey(k => k.Id);
            builder.Property(p => p.Id).HasColumnName("EnrollmentID");
            builder.HasOne(p => p.Student).WithMany(p => p.Enrollments);
            builder.HasOne(p => p.Course).WithMany();
            builder.Property(p => p.Grade);
        }
    }
}
