using BeyondNet.Ddd.ValueObjects.Validators;

namespace BeyondNet.Ddd.ValueObjects
{
    public class DateTimeUtcValueObject : ValueObject<DateTime>
    {
        protected DateTimeUtcValueObject(DateTime value):base(value)
        {         
           
        }

        public static DateTimeUtcValueObject Create(DateTime value)
        {
            return new DateTimeUtcValueObject(value);
        }

        public override void AddValidators()
        {
            base.AddValidators();

            AddValidator(new DateTimeUtcValidator(this));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static DateTimeUtcValueObject DefaultValue => new DateTimeUtcValueObject(DateTime.UtcNow);
    }
}
