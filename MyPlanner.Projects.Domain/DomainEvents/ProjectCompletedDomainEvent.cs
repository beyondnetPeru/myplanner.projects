using BeyondNet.Ddd;


namespace MyPlanner.Projects.Domain.DomainEvents
{
    public class ProjectCompletedDomainEvent : DomainEvent
    {
        public string ProjectId { get; }
        public string ProjectName { get; }
        public string Status { get; }

        public ProjectCompletedDomainEvent(string projectId, string projectName)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            Status = ProjectStatus.Completed.Name;
        }
    }
}
