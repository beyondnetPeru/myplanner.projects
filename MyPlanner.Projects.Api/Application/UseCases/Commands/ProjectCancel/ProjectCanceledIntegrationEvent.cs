using MyPlanner.EventBus.Events;
using MyPlanner.Projects.Domain;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCancel
{
    public record ProjectCanceledIntegrationEvent : IntegrationEvent
    {
        public string ProjectId { get; }
        public string ProjectName { get; }
        public string Status { get; }

        public ProjectCanceledIntegrationEvent(string projectId, string name)
        {
            ProjectId = projectId;
            ProjectName = name;
            Status = ProjectStatus.Canceled.Name;
        }
    }
}
