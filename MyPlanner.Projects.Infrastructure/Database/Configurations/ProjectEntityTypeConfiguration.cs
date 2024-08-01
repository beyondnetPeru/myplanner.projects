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
            builder.Property(p => p.Product).IsRequired(false);
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Description).IsRequired(false);
            builder.Property(p => p.Owner).IsRequired(false);
            builder.Property(p => p.RiskLevel).IsRequired(false);

            builder.OwnsOne(p => p.Audit).Property(p => p.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            builder.OwnsOne(p => p.Audit).Property(p => p.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.OwnsOne(p => p.Audit).Property(p => p.UpdatedBy).HasColumnName("UpdatedBy").IsRequired(false);
            builder.OwnsOne(p => p.Audit).Property(p => p.UpdatedAt).HasColumnName("UpdatedAt").IsRequired(false);
            builder.OwnsOne(p => p.Audit).Property(p => p.TimeSpan).HasColumnName("TimeSpan").IsRequired(false);

            builder.Property(p => p.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.HasMany(p => p.Backlogs)
                .WithOne(p => p.Project)
                .HasForeignKey(b => b.ProjectId)
                .HasPrincipalKey(p => p.Id);

            builder.HasMany(p => p.Tracks)
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
