using MyPlanner.Projects.Infrastructure.Database.Tables;

namespace MyPlanner.Projects.Api.Application.UseCases.Queries
{
    public record ProjectInfo
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
        public string Status { get; set; }
        public ICollection<BacklogInfo> Backlogs { get; set; } = new List<BacklogInfo>();
        public ICollection<StakeHolderInfo> StakeHolders { get; set; } = new List<StakeHolderInfo>();
        public ICollection<ScopeInfo>? Scopes { get; set; } = new List<ScopeInfo>();
    }

    public record BacklogInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<FeatureInfo> Features { get; set; } = new List<FeatureInfo>();
        public string Status { get; set; }
    }

    public record FeatureInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
    }


    public record ScopeInfo
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime RegisterDate { get; set; }
    }

    public record StakeHolderInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Rol { get; set; }
        public string Email { get; set; }
    }

    public record ProjectSummary
    {
        public string Id  { get; init; }
        public string Name { get; init; }
        public DateTime Date { get; init; }
        public string Status { get; init; }        
    }
}
