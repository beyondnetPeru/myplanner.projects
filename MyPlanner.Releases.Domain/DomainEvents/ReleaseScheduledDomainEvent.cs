using BeyondNet.Ddd;
using MyPlanner.Releases.Domain;

namespace MyProjects.Domain.ReleaseAggregate.Events
{
    public class ReleaseScheduledDomainEvent : DomainEvent
    {
        public ReleaseScheduledDomainEvent(string id, string name, DateTime goLiveDate)
        {
            GoLiveDate = goLiveDate;
            Id = id;
            Name = name;
        }

        public DateTime GoLiveDate { get; }
        public string Id { get; }
        public string Name { get; }

        public string Status { get; set; } = ReleaseStatus.Scheduled.Name;
    }
}
