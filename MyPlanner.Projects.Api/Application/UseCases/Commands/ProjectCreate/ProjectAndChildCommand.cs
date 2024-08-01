using MediatR;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCreate
{
    public class ProjectCreateAndChildCommand : IRequest<bool>
    {
        public ProjectDto Project { get; }

        public ProjectCreateAndChildCommand(ProjectDto project)
        {
            Project = project;
        }
    }
}
    
   