using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyPlanner.Projects.Infrastructure.Database.Tables;

namespace MyPlanner.Projects.Infrastructure.Database.Configurations
{
    public class BacklogEntityTypeConfiguration : IEntityTypeConfiguration<BacklogTable>
    {
        public void Configure(EntityTypeBuilder<BacklogTable> builder)
        {
            builder.ToTable("backlogs");

            builder.HasKey(p => p.Id);

            builder.HasMany(p => p.Features)
                .WithOne(p => p.Backlog)
                .HasForeignKey(b => b.BacklogId)
                .HasPrincipalKey(p => p.Id);
        }
    }
}
