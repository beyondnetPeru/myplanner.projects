using MediatR;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectOnHold
{
    public class ProjectOnHoldCommand : IRequest<bool>
    {
        public string ProjectId { get; }

        public ProjectOnHoldCommand(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
