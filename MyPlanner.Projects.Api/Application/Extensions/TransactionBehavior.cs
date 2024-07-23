using MediatR;
using Microsoft.EntityFrameworkCore;
using MyPlanner.Projects.Api.Application.Services;
using MyPlanner.Projects.Infrastructure.Database;

namespace MyPlanner.Projects.Api.Application.Extensions
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> logger;
        private readonly ProjectDbContext dbContext;
        private readonly IProjectIntegrationEventService projectIntegrationEventService;

        public TransactionBehavior(ProjectDbContext dbContext,
            IProjectIntegrationEventService projectIntegrationEventService,
            ILogger<TransactionBehavior<TRequest, TResponse>> logger)
        {
            this.dbContext = dbContext ?? throw new ArgumentException(nameof(ProjectDbContext));
            this.projectIntegrationEventService = projectIntegrationEventService ?? throw new ArgumentException(nameof(projectIntegrationEventService));
            this.logger = logger ?? throw new ArgumentException(nameof(ILogger));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = default(TResponse);
            var typeName = request.GetGenericTypeName();

            try
            {
                if (dbContext.HasActiveTransaction)
                {
                    return await next();
                }

                var strategy = dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    Guid transactionId;

                    await using var transaction = await dbContext.BeginTransactionAsync();
                    using (logger.BeginScope(new List<KeyValuePair<string, object>> { new("TransactionContext", transaction.TransactionId) }))
                    {
                        logger.LogInformation("Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

                        response = await next();

                        logger.LogInformation("Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                        await dbContext.CommitTransactionAsync(transaction);

                        transactionId = transaction.TransactionId;
                    }

                    await projectIntegrationEventService.PublishEventsThroughEventBusAsync(transactionId);
                });

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error Handling transaction for {CommandName} ({@Command})", typeName, request);

                throw;
            }
        }
    }
}
