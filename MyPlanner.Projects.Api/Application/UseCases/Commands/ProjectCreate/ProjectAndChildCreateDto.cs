using MyPlanner.Projects.Infrastructure.Database.Tables;
using MyPlanner.Shared.Infrastructure.Database;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCreate
{
    public class ProjectDto
    {
        public string? Track { get; set; }
        public string? Product { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int RiskLevel { get; set; }
        public string? Owner { get; set; }
        public string? UserId { get; set; }
        public ICollection<BacklogDto> Backlogs { get; set; } = new List<BacklogDto>();
        public ICollection<StakeHolderDto> StakeHolders { get; set; } = new List<StakeHolderDto>();
        public ICollection<ScopeDto> Scopes { get; set; } = new List<ScopeDto>();
        public ICollection<BudgetTable> Budgets { get; set; } = new List<BudgetTable>();
    }


    public class BacklogDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<FeatureTable> Features { get; set; } = new List<FeatureTable>();
    }

    public class FeatureDto
    {
        public string BacklogId { get; set; }
        public BacklogTable Backlog { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
    }

    public class ScopeDto
    {
        public string Description { get; set; }
        public DateTime RegisterDate { get; set; }
        public AuditTable Audit { get; set; }
    }

    public class StakeHolderDto
    {
        public string Name { get; set; }
        public AuditTable Audit { get; set; }
    }

    public class BudgetDto
    {
        public double Amount { get; set; }
        public int Symbol { get; set; }
        public string Description { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
