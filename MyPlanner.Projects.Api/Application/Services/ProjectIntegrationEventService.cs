using MyPlanner.EventBus.Abstractions;
using MyPlanner.EventBus.Events;
using MyPlanner.IntegrationEventLogEF.Services;
using MyPlanner.Projects.Infrastructure.Database;

namespace MyPlanner.Projects.Api.Application.Services
{
    public class ProjectIntegrationEventService : IProjectIntegrationEventService
    {
        private readonly IEventBus eventBus;
        private readonly ProjectDbContext projectDbContext;
        private readonly IIntegrationEventLogService integrationEventLogService;
        private readonly ILogger<ProjectIntegrationEventService> logger;

        public ProjectIntegrationEventService(IEventBus eventBus,
                                              ProjectDbContext projectDbContext,
                                              IIntegrationEventLogService integrationEventLogService,
                                              ILogger<ProjectIntegrationEventService> logger)
        {
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.projectDbContext = projectDbContext ?? throw new ArgumentNullException(nameof(ProjectDbContext));
            this.integrationEventLogService = integrationEventLogService ?? throw new ArgumentNullException(nameof(integrationEventLogService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task AddAndSaveEventAsync(IntegrationEvent evt)
        {
            logger.LogInformation("----- Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

            await integrationEventLogService.SaveEventAsync(evt, projectDbContext.GetCurrentTransaction());
        }

        public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
        {
            var pendingLogEvents = await integrationEventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

            foreach (var logEvt in pendingLogEvents)
            {
                logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", logEvt.EventId, logEvt.IntegrationEvent);

                try
                {
                    await integrationEventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                    await eventBus.PublishAsync(logEvt.IntegrationEvent);
                    await integrationEventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error publishing integration event: {IntegrationEventId}", logEvt.EventId);

                    await integrationEventLogService.MarkEventAsFailedAsync(logEvt.EventId);
                }
            }
        }
    }
}
