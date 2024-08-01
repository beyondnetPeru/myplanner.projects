namespace MyPlanner.Projects.Api.Application.Dtos
{
    public class FeatureTable
    {
        public string Id { get; set; }
        public string BacklogId { get; set; }
        public BacklogTable Backlog { get; set; }
        public string Name { get; set; }
        public string Track { get; set; }
        public string TechnicalScope { get; set; }
        public string BusinessScope { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public int Status { get; set; }
    }
}
