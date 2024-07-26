using MediatR;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectStart
{
    public class ProjectStartCommand : IRequest<bool>
    {
        public string ProjectId { get; }

        public ProjectStartCommand(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
