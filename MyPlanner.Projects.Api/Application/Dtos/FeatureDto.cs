namespace MyPlanner.Projects.Infrastructure.Database.Tables
{
    public class FeatureDto
    {
        public string Id { get; set; }
        public string BacklogId { get; set; }
        public BacklogDto Backlog { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public int Status { get; set; }
    }
}
