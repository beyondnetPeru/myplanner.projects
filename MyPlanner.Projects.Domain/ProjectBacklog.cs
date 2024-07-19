using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;

namespace MyPlanner.Projects.Domain
{
    public class ProjectBacklogProps : IProps
    {
        public Name Name { get; set; }
        public Description Description { get; set; } = Description.DefaultValue;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<ProjectBackLogFeature> Features { get; set; } = new List<ProjectBackLogFeature>();
        public ProjectBacklogStatus Status { get; set; }

        public ProjectBacklogProps(Name name)
        {
            Name = name;

            Status = ProjectBacklogStatus.NotStarted;
        }

        public object Clone()
        {
            return new ProjectBacklogProps(Name)
            {
                Description = Description,
                StartDate = StartDate,
                EndDate = EndDate,
                Features = Features,
                Status = Status
            };
        }
    }

    public class ProjectBacklog : Entity<ProjectBacklog, ProjectBacklogProps>
    {
        private ProjectBacklog(ProjectBacklogProps props) : base(props)
        {

        }

        public static ProjectBacklog Create(Name name, DateTime startDate, DateTime endDate)
        {
            var props = new ProjectBacklogProps(name)
            {
                StartDate = startDate,
                EndDate = endDate
            };

            return new ProjectBacklog(props);
        }

        public void UpdateName(Name name)
        {
            GetProps().Name = name;
        }

        public void UpdateStartDate(DateTime startDate)
        {
            GetProps().StartDate = startDate;
        }

        public void UpdateEndDate(DateTime endDate)
        {
            GetProps().EndDate = endDate;
        }

        public void AddFeature(ProjectBackLogFeature feature)
        {
            GetProps().Features.Add(feature);
        }

        public void RemoveFeature(ProjectBackLogFeature feature)
        {
            GetProps().Features.Remove(feature);
        }

        public void Start()
        {
            GetProps().Status = ProjectBacklogStatus.InProgress;
        }

        public void Complete()
        {
            GetProps().Status = ProjectBacklogStatus.Completed;
        }

        public void Hold()
        {
            GetProps().Status = ProjectBacklogStatus.OnHold;
        }

    }

    public class ProjectBacklogStatus : Enumeration
    {
        public static ProjectBacklogStatus NotStarted = new ProjectBacklogStatus(1, nameof(NotStarted).ToLowerInvariant());
        public static ProjectBacklogStatus InProgress = new ProjectBacklogStatus(2, nameof(InProgress).ToLowerInvariant());
        public static ProjectBacklogStatus Completed = new ProjectBacklogStatus(3, nameof(Completed).ToLowerInvariant());
        public static ProjectBacklogStatus OnHold = new ProjectBacklogStatus(4, nameof(OnHold).ToLowerInvariant());

        public ProjectBacklogStatus(int id, string name) : base(id, name)
        {
        }
    }
}
