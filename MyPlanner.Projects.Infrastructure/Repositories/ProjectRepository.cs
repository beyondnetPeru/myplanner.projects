using AutoMapper;
using BeyondNet.Ddd.Interfaces;
using MyPlanner.Projects.Domain;
using MyPlanner.Projects.Infrastructure.Database;
using MyPlanner.Projects.Infrastructure.Database.Tables;

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

        public async Task<ProjectTable> FindAsync(string id)
        {
            var projectTable = await context.Projects.FindAsync(id);

            if (projectTable == null)
            {
                throw new KeyNotFoundException($"Entity \"{nameof(ProjectTable)}\" ({id}) was not found.");
            }

            return projectTable;
        }


        public ProjectTable TransformEntityToTable(Project project)
        {
            var projectTable = new ProjectTable();

            var projectProps = project.GetPropsCopy();

            projectTable.Id = projectProps.Id.GetValue();
            projectTable.Name = projectProps.Name.GetValue();

            projectTable.Audit = new AuditTable()
            {
                CreatedBy = projectProps.Audit.GetValue().CreatedBy,
                CreatedAt = projectProps.Audit.GetValue().CreatedAt,
            };
             
            return projectTable;
        }

        public async Task Add(Project project)
        {
            var projectTable = TransformEntityToTable(project);

            await this.context.AddAsync(projectTable);
        }

        public async Task UpdateName(string id, string name)
        {
            var projectTable = await FindAsync(id);

            projectTable.Name = name;
        }

        public async Task UpdateTrack(string id, string track)
        {
            var projectTable = await FindAsync(id);

            projectTable.Track = track;
        }

        public async Task UpdateDescription(string id, string description)
        {
            var projectTable = await FindAsync(id);

            projectTable.Description = description;
        }

        public async Task UpdateOwner(string id, string owner)
        {
            var projectTable = await FindAsync(id);

            projectTable.Owner = owner;
        }

        public async Task UpdateRiskLevel(string id, int riskLevel)
        {
            var projectTable = await FindAsync(id);

            projectTable.RiskLevel = riskLevel;
        }

        public async Task UpdateStatus(string id, int status)
        {
            var projectTable = await FindAsync(id);

            projectTable.Status = status;
        }

        public async Task Delete(string id)
        {
            var projectTable = await FindAsync(id);

            this.context.Remove(projectTable);
        }
    }
}
