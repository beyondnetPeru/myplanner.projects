using MediatR;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateDescription
{
    public class ProjectUpdateDescriptionCommand : IRequest<bool>
    {
        public string ProjectId { get; set; }
        public string Description { get; set; }

        public ProjectUpdateDescriptionCommand(string projectId, string description)
        {
            ProjectId = projectId;
            Description = description;
        }
    }
}
