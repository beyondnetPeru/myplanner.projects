using BeyondNet.Ddd;
using MyPlanner.Releases.Domain;

namespace MyProjects.Domain.ReleaseAggregate.Events
{
    public class ReleaseFeatureCanceledDomainEvent : DomainEvent
    {
        public string ReleaseFeatureId { get; }
        public string FeatureName { get; }

        public ReleaseFeatureCanceledDomainEvent(string releaseFeatureId, string featureName)
        {
            ReleaseFeatureId = releaseFeatureId;
            FeatureName = featureName;
        }

        public string Status { get; set; } = ReleaseFeatureStatus.Canceled.Name;
    }
}
