using MediatR;
using MyPlanner.Projects.Domain;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectOnHold
{
    public class ProjectOnHoldCommandHandler : IRequestHandler<ProjectOnHoldCommand, bool>
    {
        private readonly Logger<ProjectOnHoldCommandHandler> logger;
        private readonly IProjectRepository projectRepository;

        public ProjectOnHoldCommandHandler(Logger<ProjectOnHoldCommandHandler> logger, IProjectRepository projectRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
        }
        public async Task<bool> Handle(ProjectOnHoldCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling ProjectOnHoldCommand for project {ProjectId}", request.ProjectId);
            
            var project = await projectRepository.GetAsync(request.ProjectId);
            
            if (project == null)
            {
                logger.LogWarning("Project {ProjectId} not found", request.ProjectId);
                return false;
            }
            
            project.OnHold();

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
