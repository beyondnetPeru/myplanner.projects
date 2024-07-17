using BeyondNet.Ddd;
using MyPlanner.Releases.Domain;

namespace MyProjects.Domain.ReleaseAggregate.Events
{
    public class ReleaseClosedDomainEvent : DomainEvent
    {
        public ReleaseClosedDomainEvent(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }

        public string Status { get; set; } = ReleaseStatus.Closed.Name;
    }
}
