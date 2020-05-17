using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fingers10.EnterpriseArchitecture.Infrastructure.Data.Configuration
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Course").HasKey(k => k.Id);
            builder.Property(p => p.Id).HasColumnName("CourseID");
            builder.Property(p => p.Name);
        }
    }
}
