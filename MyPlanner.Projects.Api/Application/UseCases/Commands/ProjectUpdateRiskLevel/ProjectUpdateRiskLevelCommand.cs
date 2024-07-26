using MediatR;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateRiskLevel
{
    public class ProjectUpdateRiskLevelCommand : IRequest<bool>
    {
        public string ProjectId { get; }
        public int RiskLevel { get; }

        public ProjectUpdateRiskLevelCommand(string projectId, int riskLevel)
        {
            ProjectId = projectId;
            RiskLevel = riskLevel;
        }
    }
}
