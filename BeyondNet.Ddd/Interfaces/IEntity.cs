
using BeyondNet.Ddd.Rules.Impl;
using BeyondNet.Ddd.Rules;
using BeyondNet.Ddd.ValueObjects;
using MediatR;
using System.Collections.ObjectModel;

namespace BeyondNet.Ddd.Interfaces
{
    public interface IEntity<TEntity>
    {
        IReadOnlyCollection<INotification> DomainEvents { get; }
        IdValueObject Id { get; }
        bool IsValid { get; }
        int Version { get; }

        void AddBrokenRule(string propertyName, string message);
        void AddDomainEvent(INotification eventItem);
        void AddValidator(AbstractRuleValidator<Entity<TEntity>> validator);
        void AddValidators();
        void AddValidators(ICollection<AbstractRuleValidator<Entity<TEntity>>> validators);
        void ClearDomainEvents();
        bool Equals(object obj);
        ReadOnlyCollection<BrokenRule> GetBrokenRules();
        int GetHashCode();
        void RemoveDomainEvent(INotification eventItem);
        void RemoveValidator(AbstractRuleValidator<Entity<TEntity>> validator);
        void SetVersion(int version);
        void Validate();
    }
}