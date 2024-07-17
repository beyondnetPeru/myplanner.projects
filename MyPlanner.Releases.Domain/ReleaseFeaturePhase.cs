
using BeyondNet.Ddd;
using BeyondNet.Ddd.ValueObjects;

namespace MyPlanner.Releases.Domain
{
    public class ReleaseFeaturePhase : Entity<ReleaseFeaturePhase>
    {
        public StringValueObject Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ReleaseFeaturePhaseStatus Status { get; set; }

        private ReleaseFeaturePhase(StringValueObject name)
        {
            Name = name;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddMonths(1);
            Status = ReleaseFeaturePhaseStatus.Registered;
        }

        public static ReleaseFeaturePhase Create(StringValueObject name)
        {
            return new ReleaseFeaturePhase(name);
        }

        public void UpdateName(StringValueObject name)
        {
            Name = name;
        }

        public void UpdateStartDate(DateTime startDate)
        {
            if (startDate.Date >= EndDate.Date)
            {
                AddBrokenRule("StartDate", "Start date must be before end date");
                return;
            }

            StartDate = startDate;
        }

        public void UpdateEndDate(DateTime endDate)
        {
            if (endDate.Date <= StartDate.Date)
            {
                AddBrokenRule("EndDate", "End date must be after start date");
                return;
            }

            EndDate = endDate;
        }

        public void OnHold()
        {
            if (Status == ReleaseFeaturePhaseStatus.Canceled)
            {
                AddBrokenRule("Status", "Phase is canceled");
                return;
            }

            Status = ReleaseFeaturePhaseStatus.OnHold;
        }

        public void Cancel()
        {
            if (Status == ReleaseFeaturePhaseStatus.Canceled)
            {
                AddBrokenRule("Status", "Phase is already canceled");
                return;
            }

            Status = ReleaseFeaturePhaseStatus.Canceled;
        }

        public void Resume()
        {
            if (Status != ReleaseFeaturePhaseStatus.OnHold)
            {
                AddBrokenRule("Status", "Phase is not on hold");
                return;
            }

            Status = ReleaseFeaturePhaseStatus.Registered;
        }
    }

    public class ReleaseFeaturePhaseStatus : Enumeration
    {
        public static ReleaseFeaturePhaseStatus Registered = new ReleaseFeaturePhaseStatus(1, nameof(Registered).ToLowerInvariant());
        public static ReleaseFeaturePhaseStatus OnHold = new ReleaseFeaturePhaseStatus(2, nameof(OnHold).ToLowerInvariant());
        public static ReleaseFeaturePhaseStatus Canceled = new ReleaseFeaturePhaseStatus(3, nameof(Canceled).ToLowerInvariant());

        public ReleaseFeaturePhaseStatus(int id, string name)
            : base(id, name)
        {
        }
    }
}
