namespace BeyondNet.Ddd.Rules
{
    public class BrokenRule
    {
        public string Property { get; private set; }
        public string Message { get; private set; }

        public BrokenRule(string property, string message)
        {
            Property = property;
            Message = message;
        }
    }
}
