using MediatR;
using MyPlanner.Projects.Domain;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectComplete
{
    public class ProjectCompleteCommandHandler : IRequestHandler<ProjectCompleteCommand, bool>
    {
        private readonly Logger<ProjectCompleteCommandHandler> logger;
        private readonly IProjectRepository projectRepository;

        public ProjectCompleteCommandHandler(Logger<ProjectCompleteCommandHandler> logger, IProjectRepository projectRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
        }

        public async Task<bool> Handle(ProjectCompleteCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling ProjectCompleteCommand for project {ProjectId}", request.ProjectId);

            var project = await projectRepository.GetAsync(request.ProjectId);

            if (project == null)
            {
                logger.LogWarning("Project {ProjectId} not found", request.ProjectId);
                return false;
            }

            project.Complete();

            if (!project.IsValid)
            {
                logger.LogWarning("Project {ProjectId} is not valid. Errors: {errors}", request.ProjectId, project.GetBrokenRules().ToString());
                return false;
            }

            await projectRepository.UpdateStatus(project.GetPropsCopy().Id.GetValue(), project.GetPropsCopy().Status.Id);

            return true;

        }
    }
}
