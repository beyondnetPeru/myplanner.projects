namespace BeyondNet.Ddd.Rules
{
    public class RuleContext
    {
        public List<(string, object)> Parameters { get; } = new List<(string, object)>();
    }
}
