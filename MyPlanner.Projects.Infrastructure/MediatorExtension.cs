using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using MediatR;
using MyPlanner.Projects.Infrastructure.Database;

namespace MyPlanner.Shared.Infrastructure.Database.Extensions
{
    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, ProjectDbContext ctx) 
        {
            var domainEntities = ctx.ChangeTracker
            .Entries<IDomainEvents>()
            .Where(x => x.Entity.GetDomainEvents() != null && x.Entity.GetDomainEvents().Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.GetDomainEvents())
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}
