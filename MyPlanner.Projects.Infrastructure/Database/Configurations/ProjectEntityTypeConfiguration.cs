using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyPlanner.Projects.Infrastructure.Database.Tables;


namespace MyPlanner.Projects.Infrastructure.Database.Configurations
{
    public class ProjectEntityTypeConfiguration : IEntityTypeConfiguration<ProjectTable>
    {
        public void Configure(EntityTypeBuilder<ProjectTable> builder)
        {
            builder.ToTable("projects");

            builder.HasKey(p => p.Id);

            //value object persisted as owned entity type supported since EF Core 2.0


            builder.Property(p => p.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.HasMany(p => p.Backlogs)
                .WithOne(p => p.Project)
                .HasForeignKey(b => b.ProjectId)
                .HasPrincipalKey(p => p.Id);

            builder.HasMany(p => p.Scopes)
                .WithOne(p => p.Project)
                .HasForeignKey(s => s.ProjectId)
                .HasPrincipalKey(p => p.Id);

            builder.HasMany(p => p.StakeHolders)
                .WithOne(p => p.Project)
                .HasForeignKey(s => s.ProjectId)
                .HasPrincipalKey(p => p.Id);

        }
    }
}
