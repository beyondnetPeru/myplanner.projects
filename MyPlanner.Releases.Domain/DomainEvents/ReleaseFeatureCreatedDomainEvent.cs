using BeyondNet.Ddd;
using MyPlanner.Releases.Domain;

namespace MyProjects.Domain.ReleaseAggregate.Events
{
    public class ReleaseFeatureCreatedDomainEvent : DomainEvent
    {
        public string ReleaseFeatureId { get; }
        public string FeatureName { get; }

        public ReleaseFeatureCreatedDomainEvent(string releaseFeatureId, string featureName)
        {
            ReleaseFeatureId = releaseFeatureId;
            FeatureName = featureName;
        }

        public string Status { get; set; } = ReleaseStatus.Created.Name;
    }
}
