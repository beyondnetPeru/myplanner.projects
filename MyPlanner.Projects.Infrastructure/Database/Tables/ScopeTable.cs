namespace MyPlanner.Projects.Infrastructure.Database.Tables
{
    public class ScopeTable
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public ProjectTable Project { get; set; }
        public string Description { get; set; }
        public DateTime RegisterDate { get; set; }
        public AuditTable Audit { get; set; }
    }
}
