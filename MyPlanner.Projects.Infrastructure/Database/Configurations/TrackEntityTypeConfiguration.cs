using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyPlanner.Projects.Infrastructure.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPlanner.Projects.Infrastructure.Database.Configurations
{
    public class TrackEntityTypeConfiguration:IEntityTypeConfiguration<TrackTable>
    {
        public void Configure(EntityTypeBuilder<TrackTable> builder)
        {
            builder.ToTable("tracks");
            builder.HasKey(x => x.Id);
        }
    }
}
