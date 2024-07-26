using MediatR;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateOwner
{
    public class ProjectUpdateOwnerCommand : IRequest<bool>
    {
        public string ProjectId { get; }
        public string Owner { get; }

        public ProjectUpdateOwnerCommand(string projectId, string owner)
        {
            ProjectId = projectId;
            Owner = owner;
        }
    }
}
