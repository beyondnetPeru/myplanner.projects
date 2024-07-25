namespace MyPlanner.Projects.Infrastructure.Database.Tables
{
    public class ProjectTable
    {
        public string Id { get; set; }
        public string? Track { get; set; }
        public string? Product { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? RiskLevel { get; set; }
        public string? Owner { get; set; }
        public int? Status { get; set; }
        public ICollection<BacklogTable> Backlogs { get; set; } = new List<BacklogTable>();
        public ICollection<StakeHolderTable> StakeHolders { get; set; } = new List<StakeHolderTable>();
        public ICollection<ScopeTable> Scopes { get; set; } = new List<ScopeTable>();
        public ICollection<BudgetTable> Budgets { get; set; } = new List<BudgetTable>();
        public AuditTable Audit { get; set; }
    }
}
