using MediatR;

namespace BeyondNet.Ddd
{
    public abstract class DomainEvent : INotification
    {
        public string EventId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public DomainEvent()
        {
            EventId = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
        }
    }
}
