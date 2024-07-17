using Ddd.ValueObjects;

namespace BeyondNet.Ddd.Test.Stubs
{
    public class FieldRequired : StringRequiredValueObject
    {
        public FieldRequired(string value) : base(value)
        {
        }

        public static FieldRequired Create(string value)
        {
            return new FieldRequired(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
