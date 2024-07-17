using BeyondNet.Ddd.Rules.Interfaces;
using System.Collections.ObjectModel;

namespace BeyondNet.Ddd.Rules.Impl
{
    public abstract class AbstractRuleValidator<T> : IRuleValidator<T> where T: class
                                                        
    {
        private List<BrokenRule> brokenRules = new List<BrokenRule>();

        public string RuleName => this.GetType().Name;
        public T? Subject { get; }

        protected AbstractRuleValidator(T subject)
        {
            Subject = subject;
        }

        public abstract void AddRules(RuleContext context);

        public ReadOnlyCollection<BrokenRule> Validate(RuleContext context) {

            if (Subject != null)
            {
                AddRules(context);
            }

            return brokenRules.AsReadOnly();
        }

        public void AddBrokenRule(string propertyName, string message)
        {
            brokenRules.Add(new BrokenRule(propertyName, message));
        }
    }
}
