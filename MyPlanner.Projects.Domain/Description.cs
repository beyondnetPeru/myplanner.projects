using BeyondNet.Ddd.ValueObjects;

namespace MyPlanner.Projects.Domain
{
    public class Description : StringValueObject
    {
        protected Description(string value) : base(value)
        {
        }

        public static new Description Create(string value)
        {
            return new Description(value);
        }

        public static Description DefaultValue => new Description(string.Empty);
    }
}
