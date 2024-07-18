
using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;
using MyProjects.Domain.ReleaseAggregate.Events;


namespace MyPlanner.Releases.Domain
{
    public class ReleaseFeatureProps : IProps
    {
        public IdValueObject Id { get; set; }
        public IdValueObject ReleaseId { get; set; }
        public StringValueObject FeatureName { get; set; }
        public StringValueObject? FeatureDescription { get; set; }
        public List<ReleaseFeaturePhase>? Phases { get; set; }
        public List<ReleaseFeatureComment>? Comments { get; set; }
        public List<ReleaseFeatureRollout>? Rollouts { get; set; }
        public ReleaseFeatureStatus FeatureStatus { get; set; }

        public ReleaseFeatureProps(IdValueObject id, IdValueObject releaseId, StringValueObject featureName)
        {
            Id = id;
            ReleaseId = releaseId;
            FeatureName = featureName;
            FeatureStatus = ReleaseFeatureStatus.Registered;

            Phases = new List<ReleaseFeaturePhase>();
            Comments = new List<ReleaseFeatureComment>();
            Rollouts = new List<ReleaseFeatureRollout>();
        }

        public object Clone()
        {
            return new ReleaseFeatureProps(Id, ReleaseId, FeatureName)
            {
                FeatureDescription = FeatureDescription,
                Phases = Phases,
                Comments = Comments,
                Rollouts = Rollouts,
                FeatureStatus = FeatureStatus
            };
        }

    }
    public class ReleaseFeature : Entity<ReleaseFeature, ReleaseFeatureProps>
    {
        private ReleaseFeature(ReleaseFeatureProps props) : base(props)
        {
            if (Tracking.IsNew)
                AddDomainEvent(new ReleaseFeatureCreatedDomainEvent(GetPropsCopy().Id.Value, GetPropsCopy().FeatureName.Value));
        }

        public static ReleaseFeature Create(IdValueObject id, IdValueObject ReleaseId, StringValueObject featureName)
        {
            var props = new ReleaseFeatureProps(id, ReleaseId, featureName);

            return new ReleaseFeature(props);
        }

        public void UpdateName(StringValueObject featureName)
        {
            Props.FeatureName = featureName;
        }

        public void UpdateDescription(StringValueObject featureDescription)
        {
            Props.FeatureDescription = featureDescription;
        }

        public void AddPhase(ReleaseFeaturePhase phase)
        {
            if (GetPropsCopy().FeatureStatus != ReleaseFeatureStatus.Registered)
            {
                AddBrokenRule("FeatureStatus", "Feature is not registered");
                return;
            }

            if (GetPropsCopy().Phases!.Any(p => p.GetPropsCopy().Name.ToString()!.Equals(phase.GetPropsCopy().Name.ToString(), StringComparison.CurrentCultureIgnoreCase)))
            {
                AddBrokenRule("PhaseName", "Phase name already exists");
                return;
            }

            Props.Phases!.Add(phase);
        }

        public void RemovePhase(ReleaseFeaturePhase phase)
        {
            if (GetPropsCopy().FeatureStatus != ReleaseFeatureStatus.Registered)
            {
                AddBrokenRule("FeatureStatus", "Feature is not registered");
                return;
            }

            if (!GetPropsCopy().Phases!.Contains(phase))
            {
                AddBrokenRule("Phase", "Phase not found");
                return;
            }

            Props.Phases!.Remove(phase);
        }

        public void AddRollout(ReleaseFeatureRollout rollout)
        {
            if (GetPropsCopy().FeatureStatus != ReleaseFeatureStatus.Registered)
            {
                AddBrokenRule("FeatureStatus", "Feature is not registered");
                return;
            }

            if (GetPropsCopy().Rollouts!.Contains(rollout))
            {
                AddBrokenRule("RolloutName", "Rollout Country already exists in the same date");
                return;
            }

            Props.Rollouts!.Add(rollout);
        }

        public void RemoveRollout(ReleaseFeatureRollout rollout)
        {
            if (GetPropsCopy().FeatureStatus != ReleaseFeatureStatus.Registered)
            {
                AddBrokenRule("FeatureStatus", "Feature is not registered");
                return;
            }

            if (!GetPropsCopy().Rollouts!.Contains(rollout))
            {
                AddBrokenRule("Rollout", "Rollout not found");
                return;
            }

            Props.Rollouts!.Remove(rollout);

        }

        public void AddComment(ReleaseFeatureComment comment)
        {

            if (GetPropsCopy().FeatureStatus == ReleaseFeatureStatus.Canceled)
            {
                AddBrokenRule("FeatureStatus", "Feature is canceled");
                return;
            }

            if (GetPropsCopy().Comments!.Contains(comment))
            {
                AddBrokenRule("Comment", "Comment already exists");
                return;
            }

            Props.Comments!.Add(comment);

        }

        public void RemoveComment(ReleaseFeatureComment comment)
        {
            if (GetPropsCopy().FeatureStatus == ReleaseFeatureStatus.Canceled)
            {
                AddBrokenRule("FeatureStatus", "Feature is canceled");
                return;
            }

            if (!GetPropsCopy().Comments!.Contains(comment))
            {
                AddBrokenRule("Comment", "Comment not found");
                return;
            }

            Props.Comments!.Remove(comment);
        }

        public void OnHold()
        {
            if (GetPropsCopy().FeatureStatus != ReleaseFeatureStatus.Registered)
            {
                AddBrokenRule("FeatureStatus", "Feature is not registered");
                return;
            }

            Props.FeatureStatus = ReleaseFeatureStatus.OnHold;

            AddDomainEvent(new ReleaseFeatureOnHoldDomainEvent(GetPropsCopy().ReleaseId.Value, GetPropsCopy().FeatureName.Value));
        }

        public void Cancel()
        {
            if (GetPropsCopy().FeatureStatus == ReleaseFeatureStatus.Canceled)
            {
                AddBrokenRule("FeatureStatus", "Feature is already canceled");
                return;
            }

            Props.FeatureStatus = ReleaseFeatureStatus.Canceled;

            AddDomainEvent(new ReleaseFeatureCanceledDomainEvent(GetPropsCopy().ReleaseId.Value, GetPropsCopy().FeatureName.Value));

        }

        public void Resume()
        {
            if (GetPropsCopy().FeatureStatus != ReleaseFeatureStatus.OnHold)
            {
                AddBrokenRule("FeatureStatus", "Feature is not on hold");
                return;
            }

            Props.FeatureStatus = ReleaseFeatureStatus.Registered;

            AddDomainEvent(new ReleaseFeatureResumeDomainEvent(GetPropsCopy().ReleaseId.Value, GetPropsCopy().FeatureName.Value));
        }
    }

    public class ReleaseFeatureStatus : Enumeration
    {
        public static ReleaseFeatureStatus Registered = new ReleaseFeatureStatus(1, nameof(Registered).ToLowerInvariant());
        public static ReleaseFeatureStatus OnHold = new ReleaseFeatureStatus(2, nameof(OnHold).ToLowerInvariant());
        public static ReleaseFeatureStatus Canceled = new ReleaseFeatureStatus(3, nameof(Canceled).ToLowerInvariant());
        public static ReleaseFeatureStatus Resumed = new ReleaseFeatureStatus(4, nameof(Resumed).ToLowerInvariant());

        public ReleaseFeatureStatus(int id, string name) : base(id, name)
        {
        }
    }
}
