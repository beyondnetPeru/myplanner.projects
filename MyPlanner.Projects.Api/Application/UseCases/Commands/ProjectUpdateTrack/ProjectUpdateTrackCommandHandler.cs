using MediatR;
using MyPlanner.Projects.Domain;
using MyPlanner.Projects.Infrastructure.Repositories;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateTrack
{
    public class ProjectUpdateTrackCommandHandler : IRequestHandler<ProjectUpdateTrackCommand, bool>
    {
        private readonly Logger<ProjectUpdateTrackCommandHandler> logger;
        private readonly IProjectRepository projectRepository;

        public ProjectUpdateTrackCommandHandler(Logger<ProjectUpdateTrackCommandHandler> logger, IProjectRepository projectRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
        }

        public async Task<bool> Handle(ProjectUpdateTrackCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating project name for project {ProjectId} to {Track}", request.ProjectId, request.Track);

            var project = await projectRepository.GetAsync(request.ProjectId);
            
            if (project == null)
            {
                logger.LogWarning("Project {ProjectId} not found", request.ProjectId);
                return false;
            }

            project.UpdateTrack(Track.Create(request.Track));

            if (!project.IsValid)
            {
                logger.LogWarning("Project name is invalid for project {ProjectId}, errors:{errors}", request.ProjectId, project.GetBrokenRules().ToString());
                return false;
            }

            await projectRepository.UpdateTrack(project.GetPropsCopy().Id.GetValue(), project.GetPropsCopy().Track!.GetValue());
            
            logger.LogInformation("Project name updated for project {ProjectId}", request.ProjectId);

            return true;

        }
    }
}
