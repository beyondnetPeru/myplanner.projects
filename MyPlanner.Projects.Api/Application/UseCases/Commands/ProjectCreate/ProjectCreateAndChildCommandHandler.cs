using BeyondNet.Ddd;
using BeyondNet.Ddd.ValueObjects;
using MediatR;
using MyPlanner.Projects.Api.Application.Services;
using MyPlanner.Projects.Domain;
using MyPlanner.Shared.Domain.ValueObjects;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCreate
{
    public class ProjectCreateAndChildCommandHandler : IRequest<ProjectCreateAndChildCommand>
    {
        private readonly IMediator mediator;
        private readonly IProjectIntegrationEventService projectIntegrationEventService;
        private readonly IProjectRepository projectRepository;
        private readonly ILogger<ProjectCreateAndChildCommandHandler> logger;

        public ProjectCreateAndChildCommandHandler(IMediator mediator,
            IProjectIntegrationEventService projectIntegrationEventService,
            IProjectRepository projectRepository,
            ILogger<ProjectCreateAndChildCommandHandler> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.projectIntegrationEventService = projectIntegrationEventService ?? throw new ArgumentNullException(nameof(projectIntegrationEventService));
            this.projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(ProjectCreateAndChildCommand request, CancellationToken cancellationToken)
        {
            var projectCreatedIntegrationEvent = new ProjectCreatedIntegrationEvent(request.Project.Name);

            await projectIntegrationEventService.AddAndSaveEventAsync(projectCreatedIntegrationEvent);

            var project = Project.Create(IdValueObject.Create(),
                                         Name.Create(request.Project.Name),
                                         Product.Create(request.Project.Product!),
                                         Description.Create(request.Project.Description!),
                                         Enumeration.FromValue<ProjectRiskLevel>(request.Project.RiskLevel!),
                                         Owner.Create(request.Project.Owner!),
                                         request.Project.Budgets.Select(budgetTable =>
                                         {
                                             return ProjectBudget.Create(IdValueObject.Create(),
                                                 null,
                                                 Price.Create(Symbol.FromValue<Symbol>(budgetTable.Symbol), budgetTable.Amount),
                                                 Description.Create(budgetTable.Description),
                                                 StringValueObject.Create(budgetTable.ApprovedBy));

                                         }).ToList(),
                                         request.Project.Backlogs.Select(backlogTable =>
                                         {
                                             return ProjectBacklog.Create(IdValueObject.Create(),
                                                                          null,
                                                                          null,
                                                                          Name.Create(backlogTable.Name),
                                                                          Description.Create(backlogTable.Description),
                                                                          DateTimeUtcValueObject.Create(backlogTable.StartDate),
                                                                          DateTimeUtcValueObject.Create(backlogTable.EndDate),
                                                                          null,
                                                                          ProjectBacklogStatus.NotStarted
                                                                          );
                                         }).ToList(),
                                         request.Project.Scopes.Select(scopeTable =>
                                         {
                                             return ProjectScope.Create(IdValueObject.Create(), null, Description.Create(scopeTable.Description), DateTimeUtcValueObject.Create(scopeTable.RegisterDate));
                                         }).ToList(),
                                         request.Project.StakeHolders.Select(stakeHolderTable =>
                                         {
                                             return ProjectStakeHolder.Create(IdValueObject.Create(), null, Name.Create(stakeHolderTable.Name));
                                         }).ToList());

            if (!project.IsValid)
            {
                logger.LogInformation($"Project is not valid. Errors: {string.Join(", ", project.GetBrokenRules().ToString())}");
                return false;
            }

            await projectRepository.Add(project);

            await projectRepository.UnitOfWork.SaveEntitiesAsync(project, cancellationToken);

            return true;
        }
    }
}
