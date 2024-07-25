using BeyondNet.Ddd.Interfaces;

namespace MyPlanner.Projects.Domain
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task Add(Project project);
        Task UpdateName(string id, string name);
        Task UpdateTrack(string id, string track);
        Task UpdateDescription(string id, string description);
        Task UpdateOwner(string id, string owner);
        Task UpdateRiskLevel(string id, int riskLevel);
        Task UpdateStatus(string id, int status);
        Task Delete(string id);
    }
}
