using MyPlanner.EventBus.Events;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCreate
{
    public record ProjectCreatedIntegrationEvent : IntegrationEvent
    {
        public string Name { get; init; }

        public ProjectCreatedIntegrationEvent(string name)
        {
            Name = name;
        }

    }
}
