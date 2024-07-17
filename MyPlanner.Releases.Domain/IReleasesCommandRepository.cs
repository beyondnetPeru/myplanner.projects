using BeyondNet.Ddd.Interfaces;
using MyProjects.Domain.ReleaseAggregate;

namespace MyPlanner.Releases.Domain
{
    public interface IReleasesCommandRepository : ICommandRepository<Release>
    {
        Task AddFeature(ReleaseFeature feature);
        Task RemoveFeature(ReleaseFeature feature);
        Task AddFeaturePhase(ReleaseFeaturePhase phase);
        Task RemoveFeaturePhase(ReleaseFeaturePhase phase);
        Task AddFeatureRollout(ReleaseFeatureRollout rollout);
        Task RemoveFeatureRollout(ReleaseFeatureRollout rollout);
        Task AddFeatureComment(ReleaseFeatureComment comment);
        Task RemoveFeatureComment(ReleaseFeatureComment comment);
        Task AddComment(ReleaseComment comment);
        Task RemoveComment(ReleaseComment comment);
        Task AddReference(ReleaseReference reference);
        Task RemoveReference(ReleaseReference reference);
    }
}
