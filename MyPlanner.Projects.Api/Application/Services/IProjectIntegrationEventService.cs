using MyPlanner.EventBus.Events;

namespace MyPlanner.Projects.Api.Application.Services
{
    public interface IProjectIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}
