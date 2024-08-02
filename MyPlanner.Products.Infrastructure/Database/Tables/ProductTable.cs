
using MyPlanner.Shared.Infrastructure.Database;

namespace MyPlanner.Products.Infrastructure.Database.Tables
{
    public class ProductTable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AuditTable Audit { get; set; }
    }
}
