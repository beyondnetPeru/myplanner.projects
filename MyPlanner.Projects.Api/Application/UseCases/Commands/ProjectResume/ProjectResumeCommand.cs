using MediatR;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectResume
{
    public class ProjectResumeCommand : IRequest<bool>
    {
        public string ProjectId { get; }

        public ProjectResumeCommand(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
