using BeyondNet.Ddd;

namespace MyPlanner.Projects.Domain
{
    public class Track : ValueObject<string>
    {
        public Track(string value) : base(value)
        {
        }

        public Track Create(string value)
        {
            return new Track(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
