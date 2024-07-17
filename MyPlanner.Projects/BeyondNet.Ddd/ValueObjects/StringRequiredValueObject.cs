using BeyondNet.Ddd.ValueObjects.Validators;

namespace BeyondNet.Ddd.ValueObjects
{
    public class StringRequiredValueObject : ValueObject<string>
    {
        protected StringRequiredValueObject(string value) : base(value)
        {

        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override void AddValidators()
        {
            base.AddValidators();

            AddValidator(new StringRequiredValidator(this));
        }
    }
}
