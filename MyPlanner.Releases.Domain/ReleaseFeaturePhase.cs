
using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;

namespace MyPlanner.Releases.Domain
{
    public class ReleaseFeaturePhaseProps : IProps
    {
        public StringValueObject Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ReleaseFeaturePhaseStatus Status { get; set; }

        public ReleaseFeaturePhaseProps(StringValueObject name, DateTime startDate, DateTime endDate)
        {
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            Status = ReleaseFeaturePhaseStatus.Registered;
        }

        public object Clone()
        {
            return new ReleaseFeaturePhaseProps(Name, StartDate, EndDate);
        }
    }

    public class ReleaseFeaturePhase : Entity<ReleaseFeaturePhase, ReleaseFeaturePhaseProps>
    {
        private ReleaseFeaturePhase(ReleaseFeaturePhaseProps props): base(props)
        {
        }

        public static ReleaseFeaturePhase Create(StringValueObject name, DateTime startDate, DateTime endDate)
        {
            var props = new ReleaseFeaturePhaseProps(name, startDate, endDate);
            
            return new ReleaseFeaturePhase(props);
        }

        public void UpdateName(StringValueObject name)
        {
            GetProps().Name = name;
        }

        public void UpdateStartDate(DateTime startDate)
        {
            if (startDate.Date >= GetPropsCopy().EndDate.Date)
            {
                AddBrokenRule("StartDate", "Start date must be before end date");
                return;
            }

            GetProps().StartDate = startDate;
        }

        public void UpdateEndDate(DateTime endDate)
        {
            if (endDate.Date <= GetPropsCopy().StartDate.Date)
            {
                AddBrokenRule("EndDate", "End date must be after start date");
                return;
            }

            GetProps().EndDate = endDate;
        }

        public void OnHold()
        {
            if ( GetPropsCopy().Status == ReleaseFeaturePhaseStatus.Canceled)
            {
                AddBrokenRule("Status", "Phase is canceled");
                return;
            }

            GetProps().Status = ReleaseFeaturePhaseStatus.OnHold;
        }

        public void Cancel()
        {
            if (GetPropsCopy().Status == ReleaseFeaturePhaseStatus.Canceled)
            {
                AddBrokenRule("Status", "Phase is already canceled");
                return;
            }

            GetProps().Status = ReleaseFeaturePhaseStatus.Canceled;
        }

        public void Resume()
        {
            if (GetPropsCopy().Status != ReleaseFeaturePhaseStatus.OnHold)
            {
                AddBrokenRule("Status", "Phase is not on hold");
                return;
            }

            GetProps().Status = ReleaseFeaturePhaseStatus.Registered;
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
