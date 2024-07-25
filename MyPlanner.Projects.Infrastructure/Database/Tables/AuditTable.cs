namespace MyPlanner.Projects.Infrastructure.Database.Tables
{
    public class AuditTable
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public TimeSpan? TimeSpan { get; set; }
    }
}
