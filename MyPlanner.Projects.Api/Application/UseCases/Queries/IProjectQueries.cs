using MyPlanner.Shared.Application.Dtos;

namespace MyPlanner.Projects.Api.Application.UseCases.Queries
{
    public interface IProjectQueries
    {
        Task<IEnumerable<ProjectInfo>> GetProjectsAsync(PaginationDto pagination);
        Task<ProjectInfo> GetProjectAsync(string id);
    }
}
