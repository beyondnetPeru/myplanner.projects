namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCreate
{
    public class ProjectCreateDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }

        public ProjectCreateDto(string name)
        {
            Name = name;
        }
    }
}
