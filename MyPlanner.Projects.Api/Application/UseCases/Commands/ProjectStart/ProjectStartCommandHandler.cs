using MediatR;
using MyPlanner.Projects.Domain;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectStart
{
    public class ProjectStartCommandHandler : IRequestHandler<ProjectStartCommand, bool>
    {
        private readonly Logger<ProjectStartCommandHandler> logger;
        private readonly IProjectRepository projectRepository;

        public ProjectStartCommandHandler(Logger<ProjectStartCommandHandler> logger, IProjectRepository projectRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
        }

        public async Task<bool> Handle(ProjectStartCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting project with ID {ProjectId}", request.ProjectId);

            var project = await projectRepository.GetAsync(request.ProjectId);
            
            if (project == null)
            {
                logger.LogWarning("Project with ID {ProjectId} was not found", request.ProjectId);
                return false;
            }

            project.Start();

            if (!project.IsValid)
            {
                logger.LogWarning("Project with ID {ProjectId} is not valid. Errors: {errors}", request.ProjectId, project.GetBrokenRules().ToString());
                return false;
            }

            await projectRepository.UpdateStatus(project.GetPropsCopy().Id.GetValue(), project.GetPropsCopy().Status.Id);

            logger.LogInformation("Project with ID {ProjectId} was started", request.ProjectId);

            return true;
        }
    }
}
