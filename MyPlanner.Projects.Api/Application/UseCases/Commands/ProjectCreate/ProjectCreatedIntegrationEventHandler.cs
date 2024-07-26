using MediatR;
using MyPlanner.EventBus.Abstractions;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCreate
{
    public class ProjectCreatedIntegrationEventHandler : IIntegrationEventHandler<ProjectCreatedIntegrationEvent>
    {
        private readonly IMediator mediator;
        private readonly ILogger<ProjectCreatedIntegrationEventHandler> logger;

        public ProjectCreatedIntegrationEventHandler(IMediator mediator, ILogger<ProjectCreatedIntegrationEventHandler> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        public async Task Handle(ProjectCreatedIntegrationEvent @event)
        {
            logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        }
    }
}
