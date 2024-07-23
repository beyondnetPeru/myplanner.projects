using AutoMapper;
using BeyondNet.Ddd.Interfaces;
using MyPlanner.Projects.Domain;
using MyPlanner.Projects.Infrastructure.Database;

namespace MyPlanner.Projects.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectDbContext context;

        private readonly IMapper mapper;
        public IUnitOfWork UnitOfWork => context;

        public ProjectRepository(ProjectDbContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task Add(Project project)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTrack(string projectId, string track)
        {
            throw new NotImplementedException();
        }

        public Task UpdateName(string projectId, string name)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDescription(string projectId, string description)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRiskLevel(string projectId, int riskLevel)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBudget(string projectId, double budget)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOwner(string projectId, string owner)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStatus(string projectId, int status)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }
    }        
}
