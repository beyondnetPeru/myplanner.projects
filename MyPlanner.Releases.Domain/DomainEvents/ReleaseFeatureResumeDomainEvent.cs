using BeyondNet.Ddd;
using MyPlanner.Releases.Domain;


namespace MyProjects.Domain.ReleaseAggregate.Events
{
    public class ReleaseFeatureResumeDomainEvent : DomainEvent
    {
        public string ReleaseFeatureId { get; }
        public string FeatureName { get; }

        public ReleaseFeatureResumeDomainEvent(string releaseFeatureId, string featureName)
        {
            ReleaseFeatureId = releaseFeatureId;
            FeatureName = featureName;
        }

        public string Status { get; set; } = ReleaseFeatureStatus.Resumed.Name;
    }
}
