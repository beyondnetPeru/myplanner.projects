﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyPlanner.Projects.Infrastructure.Database.Tables;

namespace MyPlanner.Projects.Infrastructure.Database.Configurations
{
    public class BacklogEntityTypeConfiguration : IEntityTypeConfiguration<BacklogTable>
    {
        public void Configure(EntityTypeBuilder<BacklogTable> builder)
        {
            builder.ToTable("backlogs");

            builder.OwnsOne(p => p.Audit).Property(p => p.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            builder.OwnsOne(p => p.Audit).Property(p => p.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.OwnsOne(p => p.Audit).Property(p => p.UpdatedBy).HasColumnName("UpdatedBy").IsRequired(false);
            builder.OwnsOne(p => p.Audit).Property(p => p.UpdatedAt).HasColumnName("UpdatedAt").IsRequired(false);
            builder.OwnsOne(p => p.Audit).Property(p => p.TimeSpan).HasColumnName("TimeSpan").IsRequired(false);


            builder.HasKey(p => p.Id);

            builder.HasMany(p => p.Features)
                .WithOne(p => p.Backlog)
                .HasForeignKey(b => b.BacklogId)
                .HasPrincipalKey(p => p.Id);
        }
    }
}
