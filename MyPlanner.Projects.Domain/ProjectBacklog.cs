

using BeyondNet.Ddd;

namespace MyPlanner.Projects.Domain
{
    public class ProjectBacklog : Entity<ProjectBacklog>
    {
        public Name Name { get; set; }
        public Description Description { get; set; } = Description.DefaultValue;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<ProjectBackLogFeature> Features { get; set; } = new List<ProjectBackLogFeature>();
        public ProjectBacklogStatus Status { get; set; }

        private ProjectBacklog(Name name, DateTime startDate, DateTime endDate)
        {
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            Status = ProjectBacklogStatus.NotStarted;
        }

        public static ProjectBacklog Create(Name name, DateTime startDate, DateTime endDate)
        {
            return new ProjectBacklog(name, startDate, endDate);
        }

        public void UpdateName(Name name)
        {
            Name = name;
        }

        public void UpdateStartDate(DateTime startDate)
        {
            StartDate = startDate;
        }

        public void UpdateEndDate(DateTime endDate)
        {
            EndDate = endDate;
        }

        public void AddFeature(ProjectBackLogFeature feature)
        {
            Features.Add(feature);
        }

        public void RemoveFeature(ProjectBackLogFeature feature)
        {
            Features.Remove(feature);
        }

        public void Start()
        {
            Status = ProjectBacklogStatus.InProgress;
        }

        public void Complete()
        {
            Status = ProjectBacklogStatus.Completed;
        }

        public void Hold()
        {
            Status = ProjectBacklogStatus.OnHold;
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
