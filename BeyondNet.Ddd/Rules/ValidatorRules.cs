using BeyondNet.Ddd.Rules.Impl;
using System.Collections.ObjectModel;

namespace BeyondNet.Ddd.Rules
{
    public class ValidatorRules<T> where T : class
    {
        public List<AbstractRuleValidator<T>> _businessRules = new List<AbstractRuleValidator<T>>();

        public void Add(AbstractRuleValidator<T> rule)
        {
            ArgumentNullException.ThrowIfNull(rule, nameof(rule));

            if (!_businessRules.Any(x => x.GetType() == rule.GetType()))
            {
                _businessRules.Add(rule);
            }
        }

        public void Add(IEnumerable<AbstractRuleValidator<T>> rules)
        {
            ArgumentNullException.ThrowIfNull(rules, nameof(rules));

            foreach (var rule in rules)
            {
                Add(rule);
            }
        }

        public void Remove(AbstractRuleValidator<T> rule)
        {
            ArgumentNullException.ThrowIfNull(rule, nameof(rule));

            if (_businessRules.Contains(rule))
            {
                _businessRules.Remove(rule);
            }
        }

        public void Clear()
        {
            if (_businessRules.Any())
            {
                _businessRules.Clear();
            }
        }

        public ReadOnlyCollection<AbstractRuleValidator<T>> GetValidators() => _businessRules.ToList().AsReadOnly();

        public ReadOnlyCollection<BrokenRule> GetBrokenRules(RuleContext context = null) 
        {
            var result = new List<BrokenRule>();

            foreach (var rule in _businessRules)
            {
                var brokenRules = rule.Validate(context);

                if (brokenRules.Any())
                {
                    foreach (var brokenRule in brokenRules)
                    {
                        if (!result.Any(x => x.Property.ToLower() == brokenRule.Property.ToLower()
                                                               && x.Message.ToLower() == brokenRule.Message.ToLower()))
                        {
                            result.Add(brokenRule);
                        }
                    }
                }
            }

            return result.AsReadOnly();
        }
    }   
}


