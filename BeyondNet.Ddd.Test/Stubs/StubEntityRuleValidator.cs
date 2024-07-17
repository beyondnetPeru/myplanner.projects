using BeyondNet.Ddd.Rules;
using BeyondNet.Ddd.Rules.Impl;


namespace BeyondNet.Ddd.Test.Stubs
{
    public class StubEntityRuleValidator<T> : AbstractRuleValidator<Entity<T>>
    {
        public StubEntityRuleValidator(Entity<T> subject) : base(subject)
        {
        }

        public override void AddRules(RuleContext context)
        {
            AddBrokenRule("Value", "Value must be in UTC format");
        }
    }
}
