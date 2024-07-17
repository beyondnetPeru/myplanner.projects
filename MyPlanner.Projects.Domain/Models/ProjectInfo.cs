namespace MyPlanner.Projects.Domain.Models
{
    public class ProjectInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ProjectInfo(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
