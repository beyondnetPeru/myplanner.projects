using MyPlanner.Shared.Infrastructure.Database;

namespace MyPlanner.Projects.Infrastructure.Database.Tables
{
    public class StakeHolderTable
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public ProjectTable Project { get; set; }
        public string Name { get; set; }
        public AuditTable Audit { get; set; }
    }
}
