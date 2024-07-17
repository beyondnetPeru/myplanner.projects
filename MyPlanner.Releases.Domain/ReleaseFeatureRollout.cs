
using BeyondNet.Ddd;
using BeyondNet.Ddd.ValueObjects;

namespace MyPlanner.Releases.Domain
{
    public class ReleaseFeatureRollout : Entity<ReleaseFeatureRollout>
    {
        public StringValueObject Country { get; set; }
        public DateTime RegisterDate { get; set; }

        private ReleaseFeatureRollout(StringValueObject country, DateTime date)
        {
            Country = country;
            RegisterDate = date;
        }

        public static ReleaseFeatureRollout Create(StringValueObject country, DateTime date)
        {
            return new ReleaseFeatureRollout(country, date);
        }

        public void UpdateCountry(StringValueObject country)
        {
            Country = country;
        }

        public void UpdateDate(DateTime date)
        {
            RegisterDate = date;
        }
    }
}
