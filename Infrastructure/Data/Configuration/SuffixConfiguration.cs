using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fingers10.EnterpriseArchitecture.Infrastructure.Data.Configuration
{
    public class SuffixConfiguration : IEntityTypeConfiguration<Suffix>
    {
        public void Configure(EntityTypeBuilder<Suffix> builder)
        {
            builder.ToTable("Suffix").HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("SuffixID");
            builder.Property(p => p.Name);
        }
    }
}
