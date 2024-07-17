using BeyondNet.Ddd;

namespace MyProjects.Domain.ReleaseAggregate.Events
{
    public class ReleaseCreatedDomainEvent : DomainEvent
    {
        public ReleaseCreatedDomainEvent(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; }
        public string Description { get; }
    }
}