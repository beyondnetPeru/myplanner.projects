using BeyondNet.Ddd;

namespace MyPlanner.Projects.Domain.DomainEvents
{
    public class ProjectRiskLevelUpdatedDomainEvent : DomainEvent
    {
        public string ProjectId { get; }
        public string ProjectName { get; }
        public string RiskLevel { get; }

        public ProjectRiskLevelUpdatedDomainEvent(string projectId, string projectName, string riskLevel)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            RiskLevel = riskLevel;
        }
    }
}
