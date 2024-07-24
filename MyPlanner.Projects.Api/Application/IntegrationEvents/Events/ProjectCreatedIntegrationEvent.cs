
using MyPlanner.EventBus.Events;

namespace MyPlanner.Projects.Api.Application.IntegrationEvents.Events
{
    public record ProjectCreatedIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; init; }

        public ProjectCreatedIntegrationEvent(string userId)
            => UserId = userId;
    }
}
