using MediatR;
using BeyondNet.Ddd.Rules;
using BeyondNet.Ddd.Interfaces;
using System.Collections.ObjectModel;
using BeyondNet.Ddd.Extensions;
using BeyondNet.Ddd.Rules.Impl;
using BeyondNet.Ddd.Impl;

namespace BeyondNet.Ddd
{
    

    public abstract class Entity<TEntity, TProps> : IEntity<TEntity, TProps> where TEntity : Entity<TEntity, TProps>
            where TProps : class, IProps
    {
        #region Members         

        private List<INotification> _domainEvents = new List<INotification>();
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public ValidatorRules<Entity<TEntity, TProps>> _validatorRules = new ValidatorRules<Entity<TEntity, TProps>>();

        public BrokenRules _brokenRules = new BrokenRules();

        public Tracking Tracking = new Tracking();

        #endregion

        #region Properties

        private TProps? _props;

        private TProps Props
        {
            get { return _props!; }
            set
            {
                _props = value;
            }
        }


        public int Version { get; private set; }

        public bool IsValid => !_brokenRules.GetBrokenRules().Any();

        public bool IsNew() => Tracking.IsNew;

        public bool IsDirty() => Tracking.IsDirty;

        #endregion

        #region Constructor

        protected Entity(TProps props)
        {
            _brokenRules = new BrokenRules();

            Version = 0;

            Props = props;

            AddValidators();

            Validate();

            Tracking = Tracking.MarkNew();
        }

        #endregion


        #region Methods
        public TProps GetPropsCopy()
        {
            var copyProps = this.Props.Clone();

            return (TProps)copyProps;
        }

        public TProps GetProps()
        {
            return Props;
        }

        public void SetProps(TProps props)
        {
            Props = props;
            Tracking = Tracking.MarkDirty(props);
        }


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
            _brokenRules.Add(_validatorRules.GetBrokenRules().ToList());

            var props = GetPropsCopy().GetType().GetProperties();

            var propsBrokenRules = props.GetPropertiesBrokenRules(this.Props);

            if (propsBrokenRules.Any())
            {
                _brokenRules.Add(propsBrokenRules);
            }
        }

        public virtual void AddValidators() { }

        public void AddValidator(AbstractRuleValidator<Entity<TEntity, TProps>> validator)
        {
            _validatorRules.Add(validator);
        }

        public void AddValidators(ICollection<AbstractRuleValidator<Entity<TEntity, TProps>>> validators)
        {
            _validatorRules.Add(validators);
        }

        public void RemoveValidator(AbstractRuleValidator<Entity<TEntity, TProps>> validator)
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
            if (obj == null || !(obj is Entity<TEntity, TProps>))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Entity<TEntity, TProps> left, Entity<TEntity, TProps> right)
        {
            if (Equals(left, null))
                return Equals(right, null) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity<TEntity, TProps> left, Entity<TEntity, TProps> right)
        {
            return !(left == right);
        }

        #endregion
    }
}

