using MyPlanner.Shared.Infrastructure.Database;

namespace MyPlanner.Projects.Infrastructure.Database.Tables
{
    public class BacklogTable
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public ProjectTable Project { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<FeatureTable> Features { get; set; } = new List<FeatureTable>();
        public int Status { get; set; }
        public AuditTable Audit { get; set; }
    }
}
