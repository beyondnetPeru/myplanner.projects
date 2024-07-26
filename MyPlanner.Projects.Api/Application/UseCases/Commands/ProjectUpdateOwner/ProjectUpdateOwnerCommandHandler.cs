using MediatR;
using MyPlanner.Projects.Domain;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateOwner
{
    public class ProjectUpdateOwnerCommandHandler : IRequestHandler<ProjectUpdateOwnerCommand, bool>
    {
        private readonly Logger<ProjectUpdateOwnerCommandHandler> logger;
        private readonly IProjectRepository projectRepository;

        public ProjectUpdateOwnerCommandHandler(Logger<ProjectUpdateOwnerCommandHandler> logger, IProjectRepository projectRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
        }

        public async Task<bool> Handle(ProjectUpdateOwnerCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating project name for project {ProjectId} to {Owner}", request.ProjectId, request.Owner);

            var project = await projectRepository.GetAsync(request.ProjectId);

            if (project == null)
            {
                logger.LogWarning("Project {ProjectId} not found", request.ProjectId);
                return false;
            }

            project.UpdateOwner(Owner.Create(request.Owner));

            if (!project.IsValid)
            {
                logger.LogWarning("Project name is invalid for project {ProjectId}, errors:{errors}", request.ProjectId, project.GetBrokenRules().ToString());
                return false;
            }

            await projectRepository.UpdateOwner(project.GetPropsCopy().Id.GetValue(), project.GetPropsCopy().Owner.GetValue());

            logger.LogInformation("Project name updated for project {ProjectId}", request.ProjectId);

            return true;

        }
    }
}
