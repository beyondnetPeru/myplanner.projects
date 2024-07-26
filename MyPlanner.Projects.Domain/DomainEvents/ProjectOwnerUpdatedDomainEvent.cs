using BeyondNet.Ddd;

namespace MyPlanner.Projects.Domain.DomainEvents
{
    public class ProjectOwnerUpdatedDomainEvent : DomainEvent
    {
        public string ProjectId { get; }
        public string Owner { get; }

        public string OldOwner { get; }

        public ProjectOwnerUpdatedDomainEvent(string projectId, string owner, string oldOwner)
        {
            ProjectId = projectId;
            Owner = owner;
            OldOwner = oldOwner;

        }
    }
}
