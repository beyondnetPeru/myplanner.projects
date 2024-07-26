using MediatR;
using MyPlanner.EventBus.Abstractions;
using MyPlanner.EventBus.Extensions;


namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCancel
{
    public class ProjectCanceledIntegrationEventHandler : IIntegrationEventHandler<ProjectCanceledIntegrationEvent>
    {
        private readonly IMediator mediator;
        private readonly ILogger<ProjectCanceledIntegrationEventHandler> logger;

        public ProjectCanceledIntegrationEventHandler(IMediator mediator, ILogger<ProjectCanceledIntegrationEventHandler> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(ProjectCanceledIntegrationEvent @event)
        {
            logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, "Projects.Api", @event);

            var command = new ProjectSetCanceledStatusCommand(@event.ProjectId);

            logger.LogInformation("Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    command.GetGenericTypeName(),
                    nameof(command.projectId),
                    command.projectId,
                    command);

            await mediator.Send(command);
        }
    }
}
