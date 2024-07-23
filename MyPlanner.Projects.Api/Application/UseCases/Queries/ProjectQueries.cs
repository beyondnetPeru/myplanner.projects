using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyPlanner.Projects.Infrastructure.Database;
using MyPlanner.Shared.Application.Dtos;
using MyPlanner.Shared.Infrastructure.Database.Extensions;

namespace MyPlanner.Projects.Api.Application.UseCases.Queries
{
    public class ProjectQueries(ProjectDbContext dbContext, IMapper mapper) : IProjectQueries
    {
        public async Task<IEnumerable<ProjectInfo>> GetProjectsAsync(PaginationDto pagination)
        {
            var data = await dbContext.Projects.OrderBy(p => p.Name).Paginate(pagination).ToListAsync();

            var mappedData = mapper.Map<IEnumerable<ProjectInfo>>(data);

            return mappedData;
        }

        public async Task<ProjectInfo> GetProjectAsync(string id)
        {
            var data = await dbContext.Projects.FindAsync(id);

            var mappedData = mapper.Map<ProjectInfo>(data);

            return mappedData;
        }
    }
}
