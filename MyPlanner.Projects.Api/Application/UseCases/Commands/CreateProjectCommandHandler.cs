using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;
using MediatR;
using MyPlanner.Projects.Api.Application.IntegrationEvents.Events;
using MyPlanner.Projects.Api.Application.Services;
using MyPlanner.Projects.Api.Application.UseCases.Commands;
using MyPlanner.Projects.Domain;
using MyPlanner.Projects.Infrastructure.Idempotency;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IProjectIntegrationEventService projectIntegrationEventService;
        private readonly IProjectRepository projectRepository;
        private readonly ILogger<CreateProjectCommandHandler> logger;

        public CreateProjectCommandHandler(IMediator mediator,
            IProjectIntegrationEventService projectIntegrationEventService,
            IProjectRepository projectRepository,
            ILogger<CreateProjectCommandHandler> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.projectIntegrationEventService = projectIntegrationEventService ?? throw new ArgumentNullException(nameof(projectIntegrationEventService));
            this.projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger)) ;  
        }

        public async Task<bool> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var projectCreatedIntegrationEvent = new ProjectCreatedIntegrationEvent(request.UserId);

            await projectIntegrationEventService.AddAndSaveEventAsync(projectCreatedIntegrationEvent);

            var project = Project.Create(IdValueObject.Create(), Name.Create(request.Name));

            await projectRepository.Add(project);

            return await projectRepository.UnitOfWork.SaveEntitiesAsync(project, cancellationToken); 
        }
    }
}

public class CreateprojectIdentifiedCommandHandler : IdentifiedCommandHandler<CreateProjectCommand, bool>
{
    public CreateprojectIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager, ILogger<IdentifiedCommandHandler<CreateProjectCommand, bool>> logger) : base(mediator, requestManager, logger)
    {
    }

    protected override bool CreateResultForDuplicateRequest()
    {
        return true;
    }
}
