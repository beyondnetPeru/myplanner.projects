using System.Collections.ObjectModel;
using System.Text;

namespace BeyondNet.Ddd.Rules
{
    public class BrokenRules
    {
        private List<BrokenRule> _brokenRules = new List<BrokenRule>();

        public void Add(BrokenRule brokenRule)
        {
            ArgumentNullException.ThrowIfNull(brokenRule, nameof(brokenRule));

            if (!_brokenRules.Any(x => x.Property.ToLower() == brokenRule.Property.ToLower()
                                                 && x.Message.ToLower() == brokenRule.Message.ToLower()))
            {
                _brokenRules.Add(brokenRule);
            }
        }

        public void Add(IReadOnlyCollection<BrokenRule> brokenRules)
        {
            ArgumentNullException.ThrowIfNull(brokenRules, nameof(brokenRules));

            if (brokenRules.Any())
            {
                foreach (var brokenRule in brokenRules)
                {
                    Add(brokenRule);
                }
            }
        }

        public void Remove(BrokenRule brokenRule)
        {
            ArgumentNullException.ThrowIfNull(brokenRule, nameof(brokenRule));

            if (_brokenRules.Contains(brokenRule))
            {
                _brokenRules.Remove(brokenRule);
            }
        }

        public void Clear()
        {
            if (_brokenRules.Any())
            {
                _brokenRules.Clear();
            }
        }

        public ReadOnlyCollection<BrokenRule> GetBrokenRules()
        {
            return _brokenRules.AsReadOnly();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var brokenRule in _brokenRules)
            {
                sb.AppendLine(brokenRule.Message);
            }
            return sb.ToString();
        }
    }
}
