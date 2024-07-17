
using System.Collections.ObjectModel;

namespace BeyondNet.Ddd.Rules.Interfaces
{
    public interface IRuleValidator<T> 
    {
        T? Subject { get; }
        string RuleName { get; }
        ReadOnlyCollection<BrokenRule> Validate(RuleContext context);
    }                                                                    
}
