using BeyondNet.Ddd.Interfaces;

namespace MyPlanner.Projects.Domain
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task Add(Project project);
        Task UpdateTrack(string projectId, string track);
        Task UpdateName(string projectId, string name);
        Task UpdateDescription(string projectId, string description);
        Task UpdateRiskLevel(string projectId, int riskLevel);
        Task UpdateBudget(string projectId, double budget);
        Task UpdateOwner(string projectId, string owner);
        Task UpdateStatus(string projectId, int status);
        Task Delete(string id);
    }
}
