using BeyondNet.Ddd.Rules;
using BeyondNet.Ddd.Rules.Impl;

namespace BeyondNet.Ddd.ValueObjects.Validators
{
    public class StringRequiredValidator : AbstractRuleValidator<ValueObject<string>>
    {
        public StringRequiredValidator(ValueObject<string> subject) : base(subject)
        {
        }

        public override void AddRules(RuleContext context)
        {           
            if (string.IsNullOrEmpty(Subject!.Value!.ToString()))
            {
                AddBrokenRule("Value", "Value cannot be null or empty");
            }
        }
    }
}
