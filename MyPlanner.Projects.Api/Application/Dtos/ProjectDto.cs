namespace MyPlanner.Projects.Infrastructure.Database.Tables
{
    public class ProjectDto
    {
        public string Id { get; set; }
        public string Track { get; set; }
        public string Product { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RiskLevel { get; set; }
        public double BudgetAmount { get; set; }
        public string BudgetSymbol { get; set; }
        public double BudgetExtraSymbol { get; set; }
        public double BudgetExtraAmount { get; set; }
        public string Owner { get; set; }
        public int Status { get; set; }
        public string UserId { get; private set; }
        public ICollection<BacklogDto> Backlogs { get; set; } = new List<BacklogDto>();
        public ICollection<StakeHolderDto> StakeHolders { get; set; } = new List<StakeHolderDto>();
        public ICollection<ScopeDto> Scopes { get; set; } = new List<ScopeDto>();

    }
}
