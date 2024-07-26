using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.Rules;
using BeyondNet.Ddd.ValueObjects;

namespace MyPlanner.Projects.Domain
{
    public class ProjectBacklogProps : IProps
    {
        public IdValueObject Id { get; set; }
        public IdValueObject ProjectId { get; private set; }
        public Project Project { get; private set; }
        public Name Name { get; private set; }
        public Description Description { get; private set; } = Description.DefaultValue;
        public DateTimeUtcValueObject StartDate { get; private set; }
        public DateTimeUtcValueObject EndDate { get; private set; }
        public ICollection<ProjectBackLogFeature> Features { get; set; } = new List<ProjectBackLogFeature>();
        public ProjectBacklogStatus Status { get; set; }

        public ProjectBacklogProps(IdValueObject id,Project project, Name name)
        {
            Id = id;
            Project = project;
            ProjectId = project.GetPropsCopy().Id;
            Name = name;
            Description = Description.DefaultValue;
            StartDate = DateTimeUtcValueObject.Create(DateTime.UtcNow);
            EndDate = DateTimeUtcValueObject.Create(DateTime.UtcNow);
            Status = ProjectBacklogStatus.NotStarted;
        }

        public ProjectBacklogProps(IdValueObject id, IdValueObject projectId, Project project, Name name, 
                                   Description description, DateTimeUtcValueObject startDate, DateTimeUtcValueObject endDate, 
                                   ICollection<ProjectBackLogFeature> features, ProjectBacklogStatus status)
        {
            Id = id;
            ProjectId = projectId;
            Project = project;
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Features = features;
            Status = status;
        }

        public object Clone()
        {
            return new ProjectBacklogProps(Id, Project, Name)
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

        public static ProjectBacklog Create(IdValueObject id, Project project, Name name)
        {
            return new ProjectBacklog(new ProjectBacklogProps(id, project, name));
        }

        public static ProjectBacklog Create(IdValueObject id, IdValueObject projectId, Project project, Name name,
                                                       Description description, DateTimeUtcValueObject startDate, DateTimeUtcValueObject endDate,
                                                                                                  ICollection<ProjectBackLogFeature> features, ProjectBacklogStatus status)
        {
            return new ProjectBacklog(new ProjectBacklogProps(id, projectId, project, name, description, startDate, endDate, features, status));
        }

        public void UpdateName(Name name)
        {
            GetProps().Name.SetValue(name.GetValue());
        }

        public void UpdateStartDate(DateTimeUtcValueObject startDate)
        {
            GetProps().StartDate.SetValue(startDate.GetValue());
        }

        public void UpdateEndDate(DateTimeUtcValueObject endDate)
        {
            GetProps().EndDate.SetValue(endDate.GetValue());
        }
        public void UpdateDescription(Description description)
        {
            GetProps().Description.SetValue(description.GetValue());
        }

        public void AddFeature(ProjectBackLogFeature feature)
        {
            if (!GetPropsCopy().Features.Contains(feature))
            {
                AddBrokenRule("Features", "Feature already exists in the backlog.");
                return;
            }

            GetProps().Features.Add(feature);
        }

        public void RemoveFeature(ProjectBackLogFeature feature)
        {
            if (!GetPropsCopy().Features.Contains(feature))
            {
                AddBrokenRule("Features", "Feature does not exist in the backlog.");
                return;
            }

            GetProps().Features.Remove(feature);
        }

        public void Start()
        {
            if (GetPropsCopy().Status != ProjectBacklogStatus.NotStarted)
            {
                AddBrokenRule("Status", "Backlog is already started.");
                return;
            }

            GetProps().Status = ProjectBacklogStatus.InProgress;
        }

        public void Complete()
        {
            if (GetPropsCopy().Status != ProjectBacklogStatus.InProgress)
            {
                AddBrokenRule("Status", "Backlog is not in progress.");
                return;
            }

            GetProps().Status = ProjectBacklogStatus.Completed;
        }

        public void Hold()
        {
            if (GetPropsCopy().Status == ProjectBacklogStatus.Completed)
            {
                AddBrokenRule("Status", "Backlog is already completed.");
                return;
            }

            GetProps().Status = ProjectBacklogStatus.OnHold;
        }

        public static ICollection<ProjectBacklog> Create(List<ProjectBacklogProps> list)
        {
            return list.Select(x => new ProjectBacklog(x)).ToList();
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
