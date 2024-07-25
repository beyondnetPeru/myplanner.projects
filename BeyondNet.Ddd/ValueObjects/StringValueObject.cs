using BeyondNet.Ddd.ValueObjects.Validators;

namespace BeyondNet.Ddd.ValueObjects
{
    public class StringValueObject : ValueObject<string>
    {
        protected StringValueObject(string value) : base(value)
        {
        }

        public static StringValueObject Create(string value)
        {
            return new StringValueObject(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static StringValueObject DefaultValue => new StringValueObject(string.Empty);

    }
}
