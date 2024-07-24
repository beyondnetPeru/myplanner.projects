namespace MyPlanner.Projects.Infrastructure.Database.Tables
{
    public class ScopeDto
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public ProjectDto Project { get; set; }
        public string Description { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
