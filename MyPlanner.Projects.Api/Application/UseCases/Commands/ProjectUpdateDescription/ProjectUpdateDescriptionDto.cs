namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateDescription
{
    public class ProjectUpdateDescriptionDto
    {
        public string ProjectId { get; set; }
        public string Description { get; set; }

        public ProjectUpdateDescriptionDto(string projectId, string description)
        {
            ProjectId = projectId;
            Description = description;
        }
    }
}
