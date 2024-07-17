using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using MyProjects.Domain.ReleaseAggregate;
using MyProjects.Domain.ReleaseAggregate.Events;
using MyProjects.Domain.ReleaseAggregate.ValueObjects;
using BeyondNet.Ddd.ValueObjects;

namespace MyPlanner.Releases.Domain
{
    public class Release : Entity<Release>, IAggregateRoot
    {
        public Name Name { get; private set; }
        public Description Description { get; private set; }
        public ReleaseGoLiveDate? GoLiveDate { get; private set; }
        public Owner? Owner { get; private set; }
        public DateTimeUtcValueObject StartDate { get; private set; }
        public DateTimeUtcValueObject EndDate { get; private set; }
        public List<ReleaseFeature>? Features { get; private set; }
        public List<ReleaseReference>? References { get; private set; }
        public List<ReleaseComment>? Comments { get; private set; }
        public ReleaseVersion VersionNumber { get; private set; }
        public ReleaseStatus Status { get; private set; }
        public Audit Audit { get; set; }

        private Release(Name name)
        {
            Name = name;
            VersionNumber = ReleaseVersion.Create(StageEnum.Alpha, 0, 0, 0);
            StartDate = DateTimeUtcValueObject.Create(DateTime.Now);
            EndDate = DateTimeUtcValueObject.Create(DateTime.Now);
            Features = new List<ReleaseFeature>();
            References = new List<ReleaseReference>();
            Comments = new List<ReleaseComment>();
            Status = ReleaseStatus.Created;
            Audit = Audit.Create("default");

            AddDomainEvent(new ReleaseCreatedDomainEvent(Name.Value, Description!.Value));
        }

        public static Release Create(Name name)
        {
            return new Release(name);
        }

        public void ChangeName(Name name)
        {
            if (Status != ReleaseStatus.Created)
            {
                AddBrokenRule("Title", "Title can be changed only if Release is in Created status");
                return;
            }

            Name = name;
        }

        public void ChangeDescription(Description description)
        {
            if (Status != ReleaseStatus.Created)
            {
                AddBrokenRule("Description", "Description can be changed only if Release is in Created status");
                return;
            }

            Description = description;
        }

        public void SetPeriod(DateTimeUtcValueObject startDate, DateTimeUtcValueObject endDate)
        {
            if (Status != ReleaseStatus.Created)
            {
                AddBrokenRule("Period", "Period can be set only if Release is in Created status");
                return;
            }

            StartDate = startDate;
            EndDate = endDate;
        }

        public void Open()
        {
            if (Status != ReleaseStatus.Created)
            {
                AddBrokenRule("Status", "Release can be opened only if it is in Created status");
                return;
            }

            Status = ReleaseStatus.Open;

            AddDomainEvent(new ReleaseOpenedDomainEvent(Id.Value, Name.Value));
        }

        public void Schedule(ReleaseGoLiveDate goLiveDate)
        {
            if (Status != ReleaseStatus.Open)
            {
                AddBrokenRule("Status", "Release can be scheduled only if it is in Open status");
                return;
            }

            GoLiveDate = goLiveDate;
            Status = ReleaseStatus.Scheduled;

            AddDomainEvent(new ReleaseScheduledDomainEvent(Id.Value, Name.Value, GoLiveDate.Value));
        }

        public void Close()
        {
            if (Status != ReleaseStatus.Scheduled)
            {
                AddBrokenRule("Status", "Release can be closed only if it is in Scheduled status");
                return;
            }

            Status = ReleaseStatus.Closed;

            AddDomainEvent(new ReleaseClosedDomainEvent(Id.Value, Name.Value));
        }

        public void OnHold()
        {
            if (Status != ReleaseStatus.Scheduled)
            {
                AddBrokenRule("Status", "Release can be put on hold only if it is in Scheduled status");
                return;
            }

            Status = ReleaseStatus.OnHold;

            AddDomainEvent(new ReleaseOnHoldDomainEvent(Id.Value, Name.Value));
        }

        public void SetOwner(Owner owner)
        {
            if (Status != ReleaseStatus.Created || Status != ReleaseStatus.Open)
            {
                AddBrokenRule("Owner", "Owner can be set only for Created or Open Releases");
                return;
            }

            Owner = owner;
        }

        public void SetVersion(ReleaseVersion version)
        {
            if (Status != ReleaseStatus.Open)
            {
                AddBrokenRule("Version", "Version can be set only for Open Releases");
                return;
            }

            VersionNumber = version;

        }


        public void SetGoLiveDate(ReleaseGoLiveDate goLiveDate)
        {
            if (Status != ReleaseStatus.Scheduled)
            {
                AddBrokenRule("GoLiveDate", "GoLiveDate can be set only for Scheduled Releases");
                return;
            }

            GoLiveDate = goLiveDate;


        }

        public void ClearGoLiveDate()
        {
            if (Status != ReleaseStatus.Scheduled)
            {
                AddBrokenRule("GoLiveDate", "GoLiveDate can be cleared only for Scheduled Releases");
                return;
            }


        }


        public void AddFeature(ReleaseFeature feature)
        {
            if (Status != ReleaseStatus.Open)
            {
                AddBrokenRule("Feature", "Feature can be added only for Open Releases");
                return;
            }

            Features?.Add(feature);
        }

        public void RemoveFeature(ReleaseFeature feature)
        {
            if (Status != ReleaseStatus.Open)
            {
                AddBrokenRule("Feature", "Feature can be removed only for Open Releases");
                return;
            }

            var exists = Features?.Any(f => f == feature);

            if (!exists.HasValue || !exists.Value)
            {
                AddBrokenRule("Feature", "Feature not found");
                return;
            }

            Features?.Remove(feature);

        }

        public void AddReference(ReleaseReference reference)
        {
            if (Status != ReleaseStatus.Open || Status != ReleaseStatus.Created || Status != ReleaseStatus.Scheduled)
            {
                AddBrokenRule("Reference", "Reference can be added only for Open, Created or Scheduled Releases");
                return;
            }

            References?.Add(reference);

        }

        public void RemoveReference(ReleaseReference reference)
        {
            if (Status != ReleaseStatus.Open || Status != ReleaseStatus.Created || Status != ReleaseStatus.Scheduled)
            {
                AddBrokenRule("Reference", "Reference can be removed only for Open, Created or Scheduled Releases");
                return;
            }

            var exists = References?.Any(r => r == reference);

            if (!exists.HasValue || !exists.Value)
            {
                AddBrokenRule("Reference", "Reference not found");
                return;
            }

            References?.Remove(reference);
        }

        public void AddComment(ReleaseComment comment)
        {
            if (Status != ReleaseStatus.Open || Status != ReleaseStatus.Created || Status != ReleaseStatus.Scheduled)
            {
                AddBrokenRule("Comment", "Comment can be added only for Open, Created or Scheduled Releases");
                return;
            }

            Comments?.Add(comment);

        }

        public void RemoveComment(ReleaseComment comment)
        {
            if (Status != ReleaseStatus.Open || Status != ReleaseStatus.Created || Status != ReleaseStatus.Scheduled)
            {
                AddBrokenRule("Comment", "Comment can be removed only for Open, Created or Scheduled Releases");
                return;
            }

            var exists = Comments?.Any(c => c == comment);

            if (!exists.HasValue || !exists.Value)
            {
                AddBrokenRule("Comment", "Comment not found");
                return;
            }

            Comments?.Remove(comment);
        }
    };

    public class ReleaseStatus : Enumeration
    {
        public static ReleaseStatus Created = new ReleaseStatus(1, nameof(Created).ToLowerInvariant());
        public static ReleaseStatus Open = new ReleaseStatus(2, nameof(Open).ToLowerInvariant());
        public static ReleaseStatus Scheduled = new ReleaseStatus(3, nameof(Scheduled).ToLowerInvariant());
        public static ReleaseStatus Closed = new ReleaseStatus(4, nameof(Closed).ToLowerInvariant());
        public static ReleaseStatus OnHold = new ReleaseStatus(5, nameof(OnHold).ToLowerInvariant());

        public ReleaseStatus(int id, string name) : base(id, name) { }
    }
}
