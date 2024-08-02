using MediatR;
using MyPlanner.Projects.Domain;
using MyPlanner.Shared.Domain.ValueObjects;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateName
{
    public class ProjectUpdateNameCommandHandler : IRequestHandler<ProjectUpdateNameCommand, bool>
    {
        private readonly Logger<ProjectUpdateNameCommandHandler> logger;
        private readonly IProjectRepository projectRepository;

        public ProjectUpdateNameCommandHandler(Logger<ProjectUpdateNameCommandHandler> logger, IProjectRepository projectRepository)
        {
            this.logger = logger;
            this.projectRepository = projectRepository;
        }

        public async Task<bool> Handle(ProjectUpdateNameCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating project name for project {ProjectId} to {Name}", request.ProjectId, request.Name);

            var project = await projectRepository.GetAsync(request.ProjectId);

            if (project == null)
            {
                logger.LogWarning("Project {ProjectId} not found", request.ProjectId);
                return false;
            }

            project.UpdateName(Name.Create(request.Name));

            if (!project.IsValid)
            {
                logger.LogWarning("Project name is invalid for project {ProjectId}, errors:{errors}", request.ProjectId, project.GetBrokenRules().ToString());
                return false;
            }

            await projectRepository.UpdateName(project.GetPropsCopy().Id.GetValue(), project.GetPropsCopy().Name.GetValue());

            logger.LogInformation("Project name updated for project {ProjectId}", request.ProjectId);

            return true;
        }
    }
}
