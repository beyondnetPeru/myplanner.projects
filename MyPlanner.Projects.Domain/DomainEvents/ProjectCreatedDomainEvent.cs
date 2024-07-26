using BeyondNet.Ddd;

namespace MyPlanner.Projects.Domain.DomainEvents
{
    public class ProjectCreatedDomainEvent : DomainEvent
    {
        public ProjectCreatedDomainEvent(string projectId, string name, string userId)
        {
            Name = name;
            ProjectId = projectId;
            UserId = userId;
        }

        public string Name { get; }
        public string ProjectId { get; }
        public string UserId { get; }
    }
}
