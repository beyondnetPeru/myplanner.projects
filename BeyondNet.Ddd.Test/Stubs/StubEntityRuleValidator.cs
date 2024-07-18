using BeyondNet.Ddd.Rules;
using BeyondNet.Ddd.Rules.Impl;

namespace BeyondNet.Ddd.Test.Stubs
{
    public class StubEntityRuleValidator<T> : AbstractRuleValidator<Entity<ParentRootEntity, ParentRootProps>>
    {
        public StubEntityRuleValidator(Entity<ParentRootEntity, ParentRootProps> subject) : base(subject)
        {
        }

        public override void AddRules(RuleContext context)
        {

        }
    }
}