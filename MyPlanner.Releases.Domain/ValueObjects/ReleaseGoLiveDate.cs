using BeyondNet.Ddd.ValueObjects;

namespace MyProjects.Domain.ReleaseAggregate.ValueObjects
{
    public class ReleaseGoLiveDate : DateTimeUtcValueObject
    {
        public ReleaseGoLiveDate(DateTime value) : base(value)
        {
        }
    }
}
