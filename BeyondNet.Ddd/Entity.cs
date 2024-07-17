using MediatR;
using BeyondNet.Ddd.ValueObjects;
using BeyondNet.Ddd.Rules;
using BeyondNet.Ddd.Interfaces;
using System.Collections.ObjectModel;
using BeyondNet.Ddd.Extensions;
using BeyondNet.Ddd.Rules.Impl;

namespace BeyondNet.Ddd
{
    public abstract class Entity<TEntity> : IEntity<TEntity>
    {
        #region Members         

        private List<INotification> _domainEvents = new List<INotification>();
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public ValidatorRules<Entity<TEntity>> _validatorRules = new ValidatorRules<Entity<TEntity>>();

        public BrokenRules _brokenRules = new BrokenRules();

        #endregion

        #region Properties

        public IdValueObject Id { get; private set; }

        public int Version { get; private set; }

        public bool IsValid => !_brokenRules.GetBrokenRules().Any();

        #endregion

        #region Constructor

        protected Entity()
        {
            Id = IdValueObject.Create();

            _brokenRules = new BrokenRules();

            Version = 0;

            AddValidators();

            Validate();

        }

        #endregion

        #region Methods

        public void SetVersion(int version)
        {
            if (version <= 0)
            {
                return;
            }

            Version = version;
        }

        #endregion

        #region DomainEvents                        

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        #endregion

        #region Business Rules

        public void Validate()
        {
            var entityBrokenRules = _validatorRules.GetBrokenRules();

            var valueObjectBrokenRules = GetType().GetProperties().GetPropertiesBrokenRules(this);

            var result = entityBrokenRules.Concat(valueObjectBrokenRules).ToList().AsReadOnly();

            _brokenRules.Add(result);
        }

        public virtual void AddValidators() { }

        public void AddValidator(AbstractRuleValidator<Entity<TEntity>> validator)
        {
            _validatorRules.Add(validator);
        }

        public void AddValidators(ICollection<AbstractRuleValidator<Entity<TEntity>>> validators)
        {
            _validatorRules.Add(validators);
        }

        public void RemoveValidator(AbstractRuleValidator<Entity<TEntity>> validator)
        {
            _validatorRules.Remove(validator);
        }

        public ReadOnlyCollection<BrokenRule> GetBrokenRules()
        {
            return _brokenRules.GetBrokenRules();
        }

        public void AddBrokenRule(string propertyName, string message)
        {
            _brokenRules.Add(new BrokenRule(propertyName, message));
        }

        #endregion

        #region Equality
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TEntity>))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            Entity<TEntity> item = (Entity<TEntity>)obj;


            return item.Id == Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Entity<TEntity> left, Entity<TEntity> right)
        {
            if (Equals(left, null))
                return Equals(right, null) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity<TEntity> left, Entity<TEntity> right)
        {
            return !(left == right);
        }

        #endregion
    }
}
