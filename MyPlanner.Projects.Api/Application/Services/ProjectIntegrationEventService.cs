using MyPlanner.EventBus.Events;

namespace MyPlanner.Projects.Api.Application.Services
{
    public class ProjectIntegrationEventService : IProjectIntegrationEventService
    {
        public Task AddAndSaveEventAsync(IntegrationEvent evt)
        {
            throw new NotImplementedException();
        }

        public Task PublishEventsThroughEventBusAsync(Guid transactionId)
        {
            throw new NotImplementedException();
        }
    }
}
