using MediatR;

namespace BeyondNet.Ddd.Interfaces
{
    public interface IDomainEvents
    {
        IReadOnlyCollection<INotification> GetDomainEvents();
        void ClearDomainEvents();
        void AddDomainEvent(INotification eventItem);
        void RemoveDomainEvent(INotification eventItem);
    }
}
