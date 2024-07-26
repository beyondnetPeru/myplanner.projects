using MediatR;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateName
{
    public class ProjectUpdateNameCommand : IRequest<bool>
    {
        public string ProjectId { get; set; }
        public string Name { get; set; }

        public ProjectUpdateNameCommand(string projectId, string name)
        {
            ProjectId = projectId;
            Name = name;
        }
    }
}
