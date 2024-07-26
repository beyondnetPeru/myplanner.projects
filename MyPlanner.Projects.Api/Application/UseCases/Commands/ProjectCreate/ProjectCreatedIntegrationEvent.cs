using MyPlanner.EventBus.Events;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCreate
{
    public record ProjectCreatedIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; init; }
        public string ProjectId { get; init; }
        public string Name { get; init; }

        public ProjectCreatedIntegrationEvent(string userId, string projectId, string name)
        {
            UserId = userId;
            ProjectId = projectId;
            Name = name;
        }

    }
}
