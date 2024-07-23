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
        }
    }
}
