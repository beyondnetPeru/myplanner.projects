
using BeyondNet.Ddd;
using BeyondNet.Ddd.ValueObjects;
using MyProjects.Domain.ReleaseAggregate.Events;


namespace MyPlanner.Releases.Domain
{
    public class ReleaseFeature : Entity<ReleaseFeature>
    {
        public IdValueObject ReleaseId { get; set; }
        public StringValueObject FeatureName { get; set; }
        public StringValueObject? FeatureDescription { get; set; }
        public List<ReleaseFeaturePhase> Phases { get; set; }
        public List<ReleaseFeatureComment> Comments { get; set; }
        public List<ReleaseFeatureRollout> Rollouts { get; set; }
        public ReleaseFeatureStatus FeatureStatus { get; set; }

        private ReleaseFeature(IdValueObject releaseId, StringValueObject featureName, StringValueObject featureDescription)
        {
            ReleaseId = releaseId;
            FeatureName = featureName;
            FeatureDescription = featureDescription;
            FeatureStatus = ReleaseFeatureStatus.Registered;

            Phases = new List<ReleaseFeaturePhase>();
            Comments = new List<ReleaseFeatureComment>();
            Rollouts = new List<ReleaseFeatureRollout>();

            AddDomainEvent(new ReleaseFeatureCreatedDomainEvent(releaseId.Value, featureName.Value));
        }

        public static ReleaseFeature Create(IdValueObject ReleaseId, StringValueObject featureName, StringValueObject featureDescription)
        {
            return new ReleaseFeature(ReleaseId, featureName, featureDescription);
        }

        public void UpdateName(StringValueObject featureName)
        {
            FeatureName = featureName;
        }

        public void UpdateDescription(StringValueObject featureDescription)
        {
            FeatureDescription = featureDescription;
        }

        public void AddPhase(ReleaseFeaturePhase phase)
        {
            if (FeatureStatus != ReleaseFeatureStatus.Registered)
            {
                AddBrokenRule("FeatureStatus", "Feature is not registered");
                return;
            }

            if (Phases.Any(p => p.Name.ToString()!.Equals(phase.Name.ToString(), StringComparison.CurrentCultureIgnoreCase)))
            {
                AddBrokenRule("PhaseName", "Phase name already exists");
                return;
            }

            Phases.Add(phase);
        }

        public void RemovePhase(ReleaseFeaturePhase phase)
        {
            if (FeatureStatus != ReleaseFeatureStatus.Registered)
            {
                AddBrokenRule("FeatureStatus", "Feature is not registered");
                return;
            }

            if (!Phases.Contains(phase))
            {
                AddBrokenRule("Phase", "Phase not found");
                return;
            }

            Phases.Remove(phase);
        }

        public void AddRollout(ReleaseFeatureRollout rollout)
        {
            if (FeatureStatus != ReleaseFeatureStatus.Registered)
            {
                AddBrokenRule("FeatureStatus", "Feature is not registered");
                return;
            }

            if (Rollouts.Contains(rollout))
            {
                AddBrokenRule("RolloutName", "Rollout Country already exists in the same date");
                return;
            }

            Rollouts.Add(rollout);
        }

        public void RemoveRollout(ReleaseFeatureRollout rollout)
        {
            if (FeatureStatus != ReleaseFeatureStatus.Registered)
            {
                AddBrokenRule("FeatureStatus", "Feature is not registered");
                return;
            }

            if (!Rollouts.Contains(rollout))
            {
                AddBrokenRule("Rollout", "Rollout not found");
                return;
            }

            Rollouts.Remove(rollout);

        }

        public void AddComment(ReleaseFeatureComment comment)
        {

            if (FeatureStatus == ReleaseFeatureStatus.Canceled)
            {
                AddBrokenRule("FeatureStatus", "Feature is canceled");
                return;
            }

            if (Comments.Contains(comment))
            {
                AddBrokenRule("Comment", "Comment already exists");
                return;
            }

            Comments.Add(comment);

        }

        public void RemoveComment(ReleaseFeatureComment comment)
        {
            if (FeatureStatus == ReleaseFeatureStatus.Canceled)
            {
                AddBrokenRule("FeatureStatus", "Feature is canceled");
                return;
            }

            if (!Comments.Contains(comment))
            {
                AddBrokenRule("Comment", "Comment not found");
                return;
            }

            Comments.Remove(comment);
        }

        public void OnHold()
        {
            if (FeatureStatus != ReleaseFeatureStatus.Registered)
            {
                AddBrokenRule("FeatureStatus", "Feature is not registered");
                return;
            }

            FeatureStatus = ReleaseFeatureStatus.OnHold;

            AddDomainEvent(new ReleaseFeatureOnHoldDomainEvent(ReleaseId.Value, FeatureName.Value));
        }

        public void Cancel()
        {
            if (FeatureStatus == ReleaseFeatureStatus.Canceled)
            {
                AddBrokenRule("FeatureStatus", "Feature is already canceled");
                return;
            }

            FeatureStatus = ReleaseFeatureStatus.Canceled;

            AddDomainEvent(new ReleaseFeatureCanceledDomainEvent(ReleaseId.Value, FeatureName.Value));

        }

        public void Resume()
        {
            if (FeatureStatus != ReleaseFeatureStatus.OnHold)
            {
                AddBrokenRule("FeatureStatus", "Feature is not on hold");
                return;
            }

            FeatureStatus = ReleaseFeatureStatus.Registered;

            AddDomainEvent(new ReleaseFeatureResumeDomainEvent(ReleaseId.Value, FeatureName.Value));
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
