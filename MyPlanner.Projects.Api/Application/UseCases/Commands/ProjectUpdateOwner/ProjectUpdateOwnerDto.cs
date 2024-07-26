namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateOwner
{
    public class ProjectUpdateOwnerDto
    {
        public string ProjectId { get; }
        public string Owner { get; }

        public ProjectUpdateOwnerDto(string projectId, string owner)
        {
            ProjectId = projectId;
            Owner = owner;
        }
    }
}
