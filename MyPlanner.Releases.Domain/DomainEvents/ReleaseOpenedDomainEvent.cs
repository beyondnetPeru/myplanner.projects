using BeyondNet.Ddd;
using MyPlanner.Releases.Domain;

namespace MyProjects.Domain.ReleaseAggregate.Events
{
    public class ReleaseOpenedDomainEvent : DomainEvent
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; } = ReleaseStatus.Open.Name;
        public ReleaseOpenedDomainEvent(string id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
