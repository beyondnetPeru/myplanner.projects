namespace MyPlanner.Projects.Infrastructure.Database.Tables
{
    public class FeatureTable
    {
        public string Id { get; set; }
        public string BacklogId { get; set; }
        public BacklogTable Backlog { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public AuditTable Audit { get; set; }
    }
}
