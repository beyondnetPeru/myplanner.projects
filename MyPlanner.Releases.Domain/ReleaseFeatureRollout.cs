
using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;

namespace MyPlanner.Releases.Domain
{
    public class ReleaseFeatureRolloutProps: IProps
    {
        public StringValueObject Country { get; set; }
        public DateTime RegisterDate { get; set; }

        public ReleaseFeatureRolloutProps(StringValueObject country, DateTime date)
        {
            Country = country;
            RegisterDate = date;
        }

        public object Clone()
        {
            return new ReleaseFeatureRolloutProps(Country, RegisterDate);
        }
    }

    public class ReleaseFeatureRollout : Entity<ReleaseFeatureRollout, ReleaseFeatureRolloutProps>
    {
        private ReleaseFeatureRollout(ReleaseFeatureRolloutProps props) : base(props)
        {
        }

        public static ReleaseFeatureRollout Create(StringValueObject country, DateTime date)
        {
            var props = new ReleaseFeatureRolloutProps(country, date);

            return new ReleaseFeatureRollout(props);
        }

        public void UpdateCountry(StringValueObject country)
        {
            GetProps().Country = country;
        }

        public void UpdateDate(DateTime date)
        {
            GetProps().RegisterDate = date;
        }
    }
}
