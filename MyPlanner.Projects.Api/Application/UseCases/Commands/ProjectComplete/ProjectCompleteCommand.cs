using MediatR;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectComplete
{
    public class ProjectCompleteCommand: IRequest<bool>
    {
        public string ProjectId { get; }

        public ProjectCompleteCommand(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
