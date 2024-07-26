namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateRiskLevel
{
    public class ProjectUpdateRiskLevelDto
    {
        public string ProjectId { get; }
        public int RiskLevel { get; }

        public ProjectUpdateRiskLevelDto(string projectId, int riskLevel)
        {
            ProjectId = projectId;
            RiskLevel = riskLevel;
        }
    }
}
