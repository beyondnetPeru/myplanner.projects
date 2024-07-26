using BeyondNet.Ddd.ValueObjects;

namespace MyPlanner.Projects.Domain
{
    public class Track : StringValueObject
    {
        protected Track(string value) : base(value)
        {
        }

        public static Track Create(string value)
        {
            return new Track(value);
        }

        public static new Track DefaultValue
        {
            get
            {
                return new Track(string.Empty);
            }
        }
    }
}
