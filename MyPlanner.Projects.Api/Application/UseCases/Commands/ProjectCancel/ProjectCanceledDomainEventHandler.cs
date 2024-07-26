using MediatR;

using MyPlanner.Projects.Api.Application.Services;
using MyPlanner.Projects.Api.Application.UseCases.Commands.CancelProject;
using MyPlanner.Projects.Domain.DomainEvents;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCancel
{
    public class ProjectCanceledDomainEventHandler : INotificationHandler<ProjectCanceledDomainEvent>
    {
        private readonly IProjectIntegrationEventService projectIntegrationEventService;

        public ProjectCanceledDomainEventHandler(IProjectIntegrationEventService projectIntegrationEventService)
        {
            this.projectIntegrationEventService = projectIntegrationEventService;
        }

        public async Task Handle(ProjectCanceledDomainEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new ProjectCanceledIntegrationEvent(notification.ProjectId, notification.ProjectName);

            await projectIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
        }
    }
}
