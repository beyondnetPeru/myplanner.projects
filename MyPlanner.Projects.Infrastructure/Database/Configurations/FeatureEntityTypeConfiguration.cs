using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyPlanner.Projects.Infrastructure.Database.Tables;

namespace MyPlanner.Projects.Infrastructure.Database.Configurations
{
    public class FeatureEntityTypeConfiguration : IEntityTypeConfiguration<FeatureTable>
    {
        public void Configure(EntityTypeBuilder<FeatureTable> builder)
        {
            builder.ToTable("features");

            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.Backlog).WithMany(x => x.Features).HasForeignKey(x => x.BacklogId);
        }
    }
}
