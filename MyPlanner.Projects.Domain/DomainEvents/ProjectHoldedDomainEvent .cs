using BeyondNet.Ddd;


namespace MyPlanner.Projects.Domain.DomainEvents
{
    public class ProjectHoldedDomainEvent : DomainEvent
    {
        public string ProjectId { get; }
        public string ProjectName { get; }
        public string Status { get; }

        public ProjectHoldedDomainEvent(string projectId, string projectName)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            Status = ProjectStatus.InProgress.Name;
        }
    }
}
