using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyPlanner.Products.Infrastructure.Database.Tables;


namespace MyPlanner.Products.Infrastructure.Database.Configurations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<ProductTable>
    {
        public void Configure(EntityTypeBuilder<ProductTable> builder)
        {
            builder.ToTable("products");
            builder.HasKey(x => x.Id);

            builder.OwnsOne(p => p.Audit).Property(p => p.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            builder.OwnsOne(p => p.Audit).Property(p => p.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.OwnsOne(p => p.Audit).Property(p => p.UpdatedBy).HasColumnName("UpdatedBy").IsRequired(false);
            builder.OwnsOne(p => p.Audit).Property(p => p.UpdatedAt).HasColumnName("UpdatedAt").IsRequired(false);
            builder.OwnsOne(p => p.Audit).Property(p => p.TimeSpan).HasColumnName("TimeSpan").IsRequired(false);
        }
    }
}
