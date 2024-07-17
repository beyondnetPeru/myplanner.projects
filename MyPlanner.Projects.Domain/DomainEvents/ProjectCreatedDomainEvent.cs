using BeyondNet.Ddd;

namespace MyPlanner.Projects.Domain.DomainEvents
{
    public class ProjectCreatedDomainEvent : DomainEvent
    {
        public ProjectCreatedDomainEvent(string id, string name)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; }
        public string Id { get; }
    }
}
