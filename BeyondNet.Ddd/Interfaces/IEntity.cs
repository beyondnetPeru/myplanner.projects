
using BeyondNet.Ddd.Rules.Impl;
using BeyondNet.Ddd.Rules;
using MediatR;
using System.Collections.ObjectModel;

namespace BeyondNet.Ddd.Interfaces
{

    public interface IEntity<TEntity, TProps>
    where TEntity : Entity<TEntity, TProps>
    where TProps : class, IProps
    {
        IReadOnlyCollection<INotification> DomainEvents { get; }
        bool IsValid { get; }
        int Version { get; }

        void AddBrokenRule(string propertyName, string message);
        void AddDomainEvent(INotification eventItem);
        void AddValidator(AbstractRuleValidator<Entity<TEntity, TProps>> validator);
        void AddValidators();
        void AddValidators(ICollection<AbstractRuleValidator<Entity<TEntity, TProps>>> validators);
        void ClearDomainEvents();
        bool Equals(object obj);
        ReadOnlyCollection<BrokenRule> GetBrokenRules();
        int GetHashCode();
        TProps GetProps();
        TProps GetPropsCopy();
        bool IsDirty();
        bool IsNew();
        void RemoveDomainEvent(INotification eventItem);
        void RemoveValidator(AbstractRuleValidator<Entity<TEntity, TProps>> validator);
        void SetProps(TProps props);
        void SetVersion(int version);
        void Validate();
    }

}