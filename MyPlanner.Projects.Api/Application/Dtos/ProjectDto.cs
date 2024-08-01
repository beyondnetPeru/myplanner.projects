using MyPlanner.Shared.Application.Dtos;

namespace MyPlanner.Projects.Api.Application.Dtos
{
    public class ProjectTable
    {
        public string Id { get; set; }
        public string Track { get; set; }
        public string Product { get; set; }
        public string Name { get; set; }
        public string TechnicalScope { get; set; }
        public string BusinessScope { get; set; }
        public string Description { get; set; }
        public ComplexityLevelEnum ComplexityLevel { get; set; }
        public int RiskLevel { get; set; }
        public double BudgetAmount { get; set; }
        public string BudgetSymbol { get; set; }
        public double BudgetExtraSymbol { get; set; }
        public double BudgetExtraAmount { get; set; }
        public string Owner { get; set; }
        public int Status { get; set; }
        public string UserId { get; private set; }
        public ICollection<BacklogTable> Backlogs { get; set; } = new List<BacklogTable>();
        public ICollection<StakeHolderTable> StakeHolders { get; set; } = new List<StakeHolderTable>();
        public ICollection<ScopeTable> Scopes { get; set; } = new List<ScopeTable>();

    }


}
