using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyPlanner.Projects.Infrastructure.Database.Tables;

namespace MyPlanner.Projects.Infrastructure.Database.Configurations
{
    public class StakeHolderEntityTypeConfiguration : IEntityTypeConfiguration<StakeHolderTable>
    {
        public void Configure(EntityTypeBuilder<StakeHolderTable> builder)
        {
            builder.ToTable("stakeholders");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Project).WithMany(x => x.StakeHolders).HasForeignKey(x => x.ProjectId);

            builder.OwnsOne(p => p.Audit).Property(p => p.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            builder.OwnsOne(p => p.Audit).Property(p => p.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.OwnsOne(p => p.Audit).Property(p => p.UpdatedBy).HasColumnName("UpdatedBy").IsRequired(false);
            builder.OwnsOne(p => p.Audit).Property(p => p.UpdatedAt).HasColumnName("UpdatedAt").IsRequired(false);
            builder.OwnsOne(p => p.Audit).Property(p => p.TimeSpan).HasColumnName("TimeSpan").IsRequired(false);

        }
    }
}
