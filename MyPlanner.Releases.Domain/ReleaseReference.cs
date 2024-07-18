using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;

namespace MyPlanner.Releases.Domain
{
    public class ReleaseReferenceProps : IProps
    {
        public IdValueObject ReleaseId { get; set; }
        public StringValueObject ReferenceName { get; set; }
        public StringValueObject ReferenceUrl { get; set; }

        public ReleaseReferenceProps(IdValueObject releaseId, StringValueObject referenceName, StringValueObject referenceUrl)
        {
            ReleaseId = releaseId;
            ReferenceName = referenceName;
            ReferenceUrl = referenceUrl;
        }

        public object Clone()
        {
            return new ReleaseReferenceProps(ReleaseId, ReferenceName, ReferenceUrl);
        }
    }

    public class ReleaseReference : Entity<ReleaseReference, ReleaseReferenceProps>
    {
        private ReleaseReference(ReleaseReferenceProps props) : base(props)
        {

        }

        public static ReleaseReference Create(IdValueObject ReleaseId, StringValueObject referenceName, StringValueObject referenceUrl)
        {
            var props = new ReleaseReferenceProps(ReleaseId, referenceName, referenceUrl);

            return new ReleaseReference(props);
        }

    }
}
