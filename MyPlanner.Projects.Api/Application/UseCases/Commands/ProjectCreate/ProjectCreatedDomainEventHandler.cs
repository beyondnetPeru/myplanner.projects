using MediatR;
using MyPlanner.Projects.Api.Application.Services;
using MyPlanner.Projects.Domain.DomainEvents;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCreate
{
    public class ProjectCreatedDomainEventHandler : INotificationHandler<ProjectCreatedDomainEvent>
    {

        private readonly IProjectIntegrationEventService projectIntegrationEventService;

        public ProjectCreatedDomainEventHandler(IProjectIntegrationEventService projectIntegrationEventService)
        {
            this.projectIntegrationEventService = projectIntegrationEventService ?? throw new ArgumentNullException(nameof(projectIntegrationEventService));
        }

        public async Task Handle(ProjectCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new ProjectCreatedIntegrationEvent(notification.UserId, notification.ProjectId, notification.Name);

            await projectIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
        }
    }
}
