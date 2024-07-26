using BeyondNet.Ddd;
using MediatR;
using MyPlanner.Projects.Domain;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateRiskLevel
{
    public class ProjectUpdateRiskLevelCommandHandler : IRequestHandler<ProjectUpdateRiskLevelCommand, bool>
    {
        private readonly Logger<ProjectUpdateRiskLevelCommandHandler> logger;
        private readonly IProjectRepository projectRepository;

        public ProjectUpdateRiskLevelCommandHandler(Logger<ProjectUpdateRiskLevelCommandHandler> logger, IProjectRepository projectRepository)
        {
            this.logger = logger;
            this.projectRepository = projectRepository;
        }

        public async Task<bool> Handle(ProjectUpdateRiskLevelCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating project name for project {ProjectId} to {RiskLevel}", request.ProjectId, request.RiskLevel);

            var project = await projectRepository.GetAsync(request.ProjectId);

            if (project == null)
            {
                logger.LogWarning("Project {ProjectId} not found", request.ProjectId);
                return false;
            }

            project.UpdateRiskLevel(Enumeration.FromValue<ProjectRiskLevel>(request.RiskLevel));

            if (!project.IsValid)
            {
                logger.LogWarning("Project name is invalid for project {ProjectId}, errors:{errors}", request.ProjectId, project.GetBrokenRules().ToString());
                return false;
            }

            await projectRepository.UpdateRiskLevel(project.GetPropsCopy().Id.GetValue(), project.GetPropsCopy().RiskLevel.Id);

            logger.LogInformation("Project name updated for project {ProjectId}", request.ProjectId);

            return true;
        }
    }
}
