using MediatR;
using MyPlanner.Projects.Domain;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateDescription
{
    public class ProjectUpdateDescriptionCommandHandler : IRequestHandler<ProjectUpdateDescriptionCommand, bool>
    {
        private readonly Logger<ProjectUpdateDescriptionCommandHandler> logger;
        private readonly IProjectRepository projectRepository;

        public ProjectUpdateDescriptionCommandHandler(Logger<ProjectUpdateDescriptionCommandHandler> logger, IProjectRepository projectRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
        }

        public async Task<bool> Handle(ProjectUpdateDescriptionCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating project name for project {ProjectId} to {Description}", request.ProjectId, request.Description);

            var project = await projectRepository.GetAsync(request.ProjectId);

            if (project == null)
            {
                logger.LogWarning("Project {ProjectId} not found", request.ProjectId);
                return false;
            }

            project.UpdateDescription(Description.Create(request.Description));

            if (!project.IsValid)
            {
                logger.LogWarning("Project name is invalid for project {ProjectId}, errors:{errors}", request.ProjectId, project.GetBrokenRules().ToString());
                return false;
            }

            await projectRepository.UpdateDescription(project.GetPropsCopy().Id.GetValue(), project.GetPropsCopy().Description!.GetValue());

            logger.LogInformation("Project name updated for project {ProjectId}", request.ProjectId);

            return true;

        }
    }
}
