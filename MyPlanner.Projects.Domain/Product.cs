

using BeyondNet.Ddd;

namespace MyPlanner.Projects.Domain
{
    public class Product : ValueObject<string>
    {
        public Product(string value) : base(value)
        {
        }

        public override string ToString() => Value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Product DefaultValue => new Product("Default");
    }
}
