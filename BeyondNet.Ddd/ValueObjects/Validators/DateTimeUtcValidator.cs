using BeyondNet.Ddd.Rules;
using BeyondNet.Ddd.Rules.Impl;

namespace BeyondNet.Ddd.ValueObjects.Validators
{
    public class DateTimeUtcValidator : AbstractRuleValidator<ValueObject<DateTime>>
    {
        public DateTimeUtcValidator(ValueObject<DateTime> subject) : base(subject)
        {
        }

        public override void AddRules(RuleContext context)
        {
            if (Subject!.Value.Kind != DateTimeKind.Utc)
            {
                AddBrokenRule("Value", "Value must be in UTC format");
            }

            if (Subject.Value < DateTime.Now.AddDays(1))
            {
                AddBrokenRule("Value", "Value must be in the future");
            }
        }
    }
}
