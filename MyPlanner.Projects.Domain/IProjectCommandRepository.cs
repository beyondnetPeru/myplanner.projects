using BeyondNet.Ddd.Interfaces;

namespace MyPlanner.Projects.Domain
{
    public interface IProjectCommandRepository : ICommandRepository<Project>
    {
        Task AddBacklog(ProjectBacklog backlog, CancellationToken cancellationToken = default);
        Task UpdateBacklog(ProjectBacklog backlog, CancellationToken cancellationToken = default);
        Task DeleteBacklog(ProjectBacklog backlog, CancellationToken cancellationToken = default);
        Task AddFeature(ProjectBackLogFeature feature, CancellationToken cancellationToken = default);
        Task DeleteFeature(ProjectBackLogFeature feature, CancellationToken cancellationToken = default);
        Task UpdateFeature(ProjectBackLogFeature feature, CancellationToken cancellationToken = default);
    }
}
