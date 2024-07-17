using BeyondNet.Ddd.Interfaces;
using MyProjects.Domain.ReleaseAggregate;

namespace MyPlanner.Releases.Domain
{
    public interface IReleasesQueryRepository : IQueryRepository<Release>
    {
        Task<ReleaseFeature> GetFeature(string ReleaseId, string featureId);
        Task<IEnumerable<ReleaseFeature>> GetFeatures(string ReleaseId);
        Task<ReleaseFeaturePhase> GetFeaturePhase(string ReleaseId, string featureId, string phaseId);
        Task<IEnumerable<ReleaseFeaturePhase>> GetFeaturePhases(string ReleaseId);
        Task<ReleaseFeatureRollout> GetFeatureRollout(string ReleaseId, string featureId, string rolloutId);
        Task<IEnumerable<ReleaseFeatureRollout>> GetFeatureRollouts(string ReleaseId);
        Task<ReleaseFeatureComment> GetFeatureComment(string ReleaseId, string featureId, string rolloutId);
        Task<IEnumerable<ReleaseFeatureComment>> GetFeatureComments(string ReleaseId);
        Task<ReleaseComment> GetComment(string ReleaseId, string commentId);
        Task<IEnumerable<ReleaseComment>> GetComments(string ReleaseId);
        Task<ReleaseReference> GetReference(string ReleaseId, string referenceId);
        Task<IEnumerable<ReleaseReference>> GetReferences(string ReleaseId);
    }
}
