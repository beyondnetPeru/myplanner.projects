
namespace MyPlanner.Projects.Infrastructure.Database.Tables
{
    public class TrackTable
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public ProjectTable Project { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
