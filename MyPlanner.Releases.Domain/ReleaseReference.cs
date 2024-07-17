using BeyondNet.Ddd;
using BeyondNet.Ddd.ValueObjects;

namespace MyPlanner.Releases.Domain
{
    public class ReleaseReference : Entity<ReleaseReference>
    {
        public IdValueObject ReleaseId { get; set; }
        public StringValueObject ReferenceName { get; set; }
        public StringValueObject ReferenceUrl { get; set; }

        private ReleaseReference(IdValueObject releaseId, StringValueObject referenceName, StringValueObject referenceUrl)
        {
            ReleaseId = releaseId;
            ReferenceName = referenceName;
            ReferenceUrl = referenceUrl;
        }

        public static ReleaseReference Create(IdValueObject ReleaseId, StringValueObject referenceName, StringValueObject referenceUrl)
        {
            return new ReleaseReference(ReleaseId, referenceName, referenceUrl);
        }

    }
}
