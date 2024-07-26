namespace MyPlanner.Projects.Infrastructure.Database.Tables
{
    public class BudgetTable
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public ProjectTable Project { get; set; }
        public double Amount { get; set; }
        public int Symbol { get; set; }
        public string Description { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime RegisterDate { get; set; }
        public AuditTable Audit { get; set; }
    }
}
