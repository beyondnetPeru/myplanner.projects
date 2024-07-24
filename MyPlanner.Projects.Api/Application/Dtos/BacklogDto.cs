﻿namespace MyPlanner.Projects.Infrastructure.Database.Tables
{
    public class BacklogDto
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public ProjectDto Project { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<FeatureDto> Features { get; set; } = new List<FeatureDto>();
        public int Status { get; set; }
    }
}
