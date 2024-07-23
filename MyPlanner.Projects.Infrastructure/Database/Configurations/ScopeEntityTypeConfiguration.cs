using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyPlanner.Projects.Infrastructure.Database.Tables;

namespace MyPlanner.Projects.Infrastructure.Database.Configurations
{
    public class ScopeEntityTypeConfiguration : IEntityTypeConfiguration<ScopeTable>
    {
        public void Configure(EntityTypeBuilder<ScopeTable> builder)
        {
            builder.ToTable("scopes");
            builder.HasKey(x => x.Id);
            
        }
    }
}
