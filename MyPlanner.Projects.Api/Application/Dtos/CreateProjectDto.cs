namespace MyPlanner.Projects.Api.Application.Dtos
{
    public class CreateProjectDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }

        public CreateProjectDto(string name)
        {
            Name = name;
        }
    }
}
