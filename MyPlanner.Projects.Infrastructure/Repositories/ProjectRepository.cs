using AutoMapper;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;
using MyPlanner.Projects.Domain;
using MyPlanner.Projects.Infrastructure.Database;
using MyPlanner.Projects.Infrastructure.Database.Tables;
using MyPlanner.Shared.Domain.ValueObjects;
using MyPlanner.Shared.Infrastructure.Database;

namespace MyPlanner.Projects.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectDbContext context;

        public IUnitOfWork UnitOfWork => context;

        public ProjectRepository(ProjectDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
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


        public ProjectTable TransFromEntityToTable(Project project)
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
            var projectTable = TransFromEntityToTable(project);

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

        public async Task<Project> GetAsync(string id)
        {
            var projectTable = await FindAsync(id);

            var project = Project.Create(IdValueObject.Create(projectTable.Id),
                Name.Create(projectTable.Name),
                Product.Create(projectTable.Product!),
                Description.Create(projectTable.Description!),
                ProjectRiskLevel.From(projectTable.RiskLevel),
                Owner.Create(projectTable.Owner!), null, null, null, null);


            project.AddStakeholder(LoadStakeHolders(projectTable, project));
            project.AddScope(LoadScopes(projectTable, project));
            project.AddTrack(LoadTracks(projectTable, project));
            project.AddBacklog(LoadBacklogs(projectTable, project));
            project.AddBudget(LoadBudgets(projectTable, project));

            return project;
        }

        private static List<ProjectStakeHolder> LoadStakeHolders(ProjectTable projectTable, Project project)
        {
            return projectTable.StakeHolders.Select(stakeHolderTable =>
            {
                return ProjectStakeHolder.Create(
                    IdValueObject.Create(stakeHolderTable.Id),
                    project,
                    Name.Create(stakeHolderTable.Name));

            }).ToList();
        }

        private static List<ProjectScope> LoadScopes(ProjectTable projectTable, Project project)
        {
            return projectTable.Scopes.Select(scopeTable =>
            {
                return ProjectScope.Create(
                               IdValueObject.Create(scopeTable.Id),
                               project,
                               Description.Create(scopeTable.Description),
                               DateTimeUtcValueObject.Create(scopeTable.RegisterDate));
            }).ToList();
        }

        private static List<ProjectFeatureTrack> LoadTracks(ProjectTable projectTable, Project project)
        {
            return projectTable.Tracks.Select(trackTable =>
            {
                return ProjectFeatureTrack.Create(
                               IdValueObject.Create(trackTable.Id),
                               project,
                               Name.Create(trackTable.Name));
            }).ToList();
        }


        private static List<ProjectBacklog> LoadBacklogs(ProjectTable projectTable, Project project)
        {
            return projectTable.Backlogs.Select(backlogTable =>
            {
                return ProjectBacklog.Create(
                                IdValueObject.Create(backlogTable.Id),
                                IdValueObject.Create(backlogTable.ProjectId),
                                project,
                                Name.Create(backlogTable.Name),
                                Description.Create(backlogTable.Description),
                                DateTimeUtcValueObject.Create(backlogTable.StartDate),
                                DateTimeUtcValueObject.Create(backlogTable.EndDate),
                                backlogTable.Features.Select(featureTable =>
                                {
                                    return ProjectBackLogFeature.Create(
                                            IdValueObject.Create(featureTable.Id),
                                            null,
                                            Name.Create(featureTable.Name),
                                            Priority.FromValue(featureTable.Priority));
                                }).ToList(),
                                ProjectBacklogStatus.FromValue<ProjectBacklogStatus>(backlogTable.Status));
            }).ToList();
        }

        private static List<ProjectBudget> LoadBudgets(ProjectTable projectTable, Project project)
        {
            return projectTable.Budgets.Select(budgetTable =>
            {
                return ProjectBudget.Create(
                    IdValueObject.Create(budgetTable.Id),
                    project,
                    Price.Create(Symbol.FromValue<Symbol>(budgetTable.Symbol), budgetTable.Amount),
                    Description.Create(budgetTable.Description),
                    StringValueObject.Create(budgetTable.ApprovedBy)
                    );
            }).ToList();
        }
    }
}
