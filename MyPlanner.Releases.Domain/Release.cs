﻿using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using MyProjects.Domain.ReleaseAggregate;
using MyProjects.Domain.ReleaseAggregate.Events;
using MyProjects.Domain.ReleaseAggregate.ValueObjects;
using BeyondNet.Ddd.ValueObjects;

namespace MyPlanner.Releases.Domain
{
    public class ReleaseProps : IProps
    {
        public IdValueObject Id { get; set; }
        public Name Name { get; set; }
        public Description? Description { get; set; }
        public ReleaseGoLiveDate? GoLiveDate { get; set; }
        public Owner? Owner { get; set; }
        public DateTimeUtcValueObject StartDate { get; set; }
        public DateTimeUtcValueObject EndDate { get; set; }
        public List<ReleaseFeature>? Features { get; set; }
        public List<ReleaseReference>? References { get; set; }
        public List<ReleaseComment>? Comments { get; set; }
        public ReleaseVersion? VersionNumber { get; set; }
        public ReleaseStatus Status { get; set; }
        public Audit Audit { get; set; }

        public ReleaseProps(IdValueObject id, Name name)
        {
            Id = id;
            Name = name;
            VersionNumber = ReleaseVersion.Create(StageEnum.Alpha, 0, 0, 0);
            StartDate = DateTimeUtcValueObject.Create(DateTime.Now);
            EndDate = DateTimeUtcValueObject.Create(DateTime.Now);
            Status = ReleaseStatus.Created;
            Audit = Audit.Create("default");
        }

        public object Clone()
        {
            return new ReleaseProps(Id, Name)
            {
                Description = Description,
                GoLiveDate = GoLiveDate,
                Owner = Owner,
                StartDate = StartDate,
                EndDate = EndDate,
                Features = Features,
                References = References,
                Comments = Comments,
                VersionNumber = VersionNumber,
                Status = Status,
                Audit = Audit
            };
        }
    }

    public class Release : Entity<Release, ReleaseProps>, IAggregateRoot
    {
        private Release(ReleaseProps props) : base(props)
        {
            if (this.Tracking.IsNew)
                AddDomainEvent(new ReleaseCreatedDomainEvent(props.Name.Value, props.Description!.Value));
        }

        public static Release Create(IdValueObject id, Name name)
        {
            var props = new ReleaseProps(id, name);

            return new Release(props);
        }

        public void ChangeName(Name name)
        {
            if (GetPropsCopy().Status != ReleaseStatus.Created)
            {
                AddBrokenRule("Title", "Title can be changed only if Release is in Created status");
                return;
            }

            Props.Name = name;
        }

        public void ChangeDescription(Description description)
        {
            if (GetPropsCopy().Status != ReleaseStatus.Created)
            {
                AddBrokenRule("Description", "Description can be changed only if Release is in Created status");
                return;
            }

            Props.Description = description;
        }

        public void SetPeriod(DateTimeUtcValueObject startDate, DateTimeUtcValueObject endDate)
        {
            if (GetPropsCopy().Status != ReleaseStatus.Created)
            {
                AddBrokenRule("Period", "Period can be set only if Release is in Created status");
                return;
            }

            Props.StartDate = startDate;
            Props.EndDate = endDate;
        }

        public void Open()
        {
            if (GetPropsCopy().Status != ReleaseStatus.Created)
            {
                AddBrokenRule("Status", "Release can be opened only if it is in Created status");
                return;
            }

            Props.Status = ReleaseStatus.Open;

            AddDomainEvent(new ReleaseOpenedDomainEvent(GetPropsCopy().Id.Value, GetPropsCopy().Name.Value));
        }

        public void Schedule(ReleaseGoLiveDate goLiveDate)
        {
            if (GetPropsCopy().Status != ReleaseStatus.Open)
            {
                AddBrokenRule("Status", "Release can be scheduled only if it is in Open status");
                return;
            }

            Props.GoLiveDate = goLiveDate;
            Props.Status = ReleaseStatus.Scheduled;

            AddDomainEvent(new ReleaseScheduledDomainEvent(GetPropsCopy().Id.Value, GetPropsCopy().Name.Value, GetPropsCopy().GoLiveDate!.Value));
        }

        public void Close()
        {
            if (GetPropsCopy().Status != ReleaseStatus.Scheduled)
            {
                AddBrokenRule("Status", "Release can be closed only if it is in Scheduled status");
                return;
            }

            Props.Status = ReleaseStatus.Closed;

            AddDomainEvent(new ReleaseClosedDomainEvent(GetPropsCopy().Id.Value, GetPropsCopy().Name.Value));
        }

        public void OnHold()
        {
            if (GetPropsCopy().Status != ReleaseStatus.Scheduled)
            {
                AddBrokenRule("Status", "Release can be put on hold only if it is in Scheduled status");
                return;
            }

            Props.Status = ReleaseStatus.OnHold;

            AddDomainEvent(new ReleaseOnHoldDomainEvent(GetPropsCopy().Id.Value, GetPropsCopy().Name.Value));
        }

        public void SetOwner(Owner owner)
        {
            if (GetPropsCopy().Status != ReleaseStatus.Created || GetPropsCopy().Status != ReleaseStatus.Open)
            {
                AddBrokenRule("Owner", "Owner can be set only for Created or Open Releases");
                return;
            }

            Props.Owner = owner;
        }

        public void SetVersion(ReleaseVersion version)
        {
            if (GetPropsCopy().Status != ReleaseStatus.Open)
            {
                AddBrokenRule("Version", "Version can be set only for Open Releases");
                return;
            }

            Props.VersionNumber = version;
        }


        public void SetGoLiveDate(ReleaseGoLiveDate goLiveDate)
        {
            if (GetPropsCopy().Status != ReleaseStatus.Scheduled)
            {
                AddBrokenRule("GoLiveDate", "GoLiveDate can be set only for Scheduled Releases");
                return;
            }

            Props.GoLiveDate = goLiveDate;
        }

        public void ClearGoLiveDate()
        {
            if (GetPropsCopy().Status != ReleaseStatus.Scheduled)
            {
                AddBrokenRule("GoLiveDate", "GoLiveDate can be cleared only for Scheduled Releases");
                return;
            }
        }


        public void AddFeature(ReleaseFeature feature)
        {
            if (GetPropsCopy().Status != ReleaseStatus.Open)
            {
                AddBrokenRule("Feature", "Feature can be added only for Open Releases");
                return;
            }

            Props.Features?.Add(feature);
        }

        public void RemoveFeature(ReleaseFeature feature)
        {
            if (GetPropsCopy().Status != ReleaseStatus.Open)
            {
                AddBrokenRule("Feature", "Feature can be removed only for Open Releases");
                return;
            }

            var exists = Props.Features?.Any(f => f == feature);

            if (!exists.HasValue || !exists.Value)
            {
                AddBrokenRule("Feature", "Feature not found");
                return;
            }

            Props.Features?.Remove(feature);

        }

        public void AddReference(ReleaseReference reference)
        {
            if (GetPropsCopy().Status != ReleaseStatus.Open || GetPropsCopy().Status != ReleaseStatus.Created || GetPropsCopy().Status != ReleaseStatus.Scheduled)
            {
                AddBrokenRule("Reference", "Reference can be added only for Open, Created or Scheduled Releases");
                return;
            }

            Props.References?.Add(reference);

        }

        public void RemoveReference(ReleaseReference reference)
        {
            if (GetPropsCopy().Status != ReleaseStatus.Open || GetPropsCopy().Status != ReleaseStatus.Created || GetPropsCopy().Status != ReleaseStatus.Scheduled)
            {
                AddBrokenRule("Reference", "Reference can be removed only for Open, Created or Scheduled Releases");
                return;
            }

            var exists = Props.References?.Any(r => r == reference);

            if (!exists.HasValue || !exists.Value)
            {
                AddBrokenRule("Reference", "Reference not found");
                return;
            }

            Props.References?.Remove(reference);
        }

        public void AddComment(ReleaseComment comment)
        {
            if (GetPropsCopy().Status != ReleaseStatus.Open || GetPropsCopy().Status != ReleaseStatus.Created || GetPropsCopy().Status != ReleaseStatus.Scheduled)
            {
                AddBrokenRule("Comment", "Comment can be added only for Open, Created or Scheduled Releases");
                return;
            }

            Props.Comments?.Add(comment);
        }

        public void RemoveComment(ReleaseComment comment)
        {
            if (GetPropsCopy().Status != ReleaseStatus.Open || GetPropsCopy().Status != ReleaseStatus.Created || GetPropsCopy().Status != ReleaseStatus.Scheduled)
            {
                AddBrokenRule("Comment", "Comment can be removed only for Open, Created or Scheduled Releases");
                return;
            }

            var exists = Props.Comments?.Any(c => c == comment);

            if (!exists.HasValue || !exists.Value)
            {
                AddBrokenRule("Comment", "Comment not found");
                return;
            }

            Props.Comments?.Remove(comment);
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