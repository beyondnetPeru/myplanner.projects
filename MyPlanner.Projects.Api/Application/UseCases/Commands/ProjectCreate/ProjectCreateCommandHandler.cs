using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;
using MediatR;
using MyPlanner.Projects.Api.Application.Services;
using MyPlanner.Projects.Api.Application.UseCases.Commands;
using MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCreate;
using MyPlanner.Projects.Domain;
using MyPlanner.Projects.Infrastructure.Idempotency;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCreate
{
    public class ProjectCreateCommandHandler : IRequestHandler<ProjectCreateCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IProjectIntegrationEventService projectIntegrationEventService;
        private readonly IProjectRepository projectRepository;
        private readonly ILogger<ProjectCreateCommandHandler> logger;

        public ProjectCreateCommandHandler(IMediator mediator,
            IProjectIntegrationEventService projectIntegrationEventService,
            IProjectRepository projectRepository,
            ILogger<ProjectCreateCommandHandler> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.projectIntegrationEventService = projectIntegrationEventService ?? throw new ArgumentNullException(nameof(projectIntegrationEventService));
            this.projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(ProjectCreateCommand request, CancellationToken cancellationToken)
        {
            var projectCreatedIntegrationEvent = new ProjectCreatedIntegrationEvent(request.Name);

            await projectIntegrationEventService.AddAndSaveEventAsync(projectCreatedIntegrationEvent);

            var project = Project.Create(IdValueObject.Create(), Name.Create(request.Name));

            await projectRepository.Add(project);

            return await projectRepository.UnitOfWork.SaveEntitiesAsync(project, cancellationToken);
        }
    }
}

public class CreateprojectIdentifiedCommandHandler : IdentifiedCommandHandler<ProjectCreateCommand, bool>
{
    public CreateprojectIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager, ILogger<IdentifiedCommandHandler<ProjectCreateCommand, bool>> logger) : base(mediator, requestManager, logger)
    {
    }

    protected override bool CreateResultForDuplicateRequest()
    {
        return true;
    }
}
