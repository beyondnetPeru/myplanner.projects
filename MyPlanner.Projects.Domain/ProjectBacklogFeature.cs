using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;
using MyPlanner.Shared.Domain.ValueObjects;

namespace MyPlanner.Projects.Domain
{
    public class ProjectBackLogFeatureProps : IProps
    {
        public IdValueObject Id { get; private set; }
        public Name Name { get; private set; }
        public Description? TechnicalScope { get; private set; } = Description.DefaultValue;
        public Description? BusinessScope { get; private set; } = Description.DefaultValue;
        public ComplexityLevel ComplexityLevel { get; private set; } = ComplexityLevel.Low;
        public ProjectBacklog Backlog { get; private set; }
        public IdValueObject BacklogId { get; private set; }
        public Description Description { get; private set; }
        public Priority Priority { get; set; }
        public BackLogFeatureStatus Status { get; set; }

        public ProjectBackLogFeatureProps(IdValueObject id,  ProjectBacklog backlog, Name name, Priority priority)
        {
            Id = id;
            Backlog = backlog;
            Name = name;
            Priority = priority;
            BacklogId = backlog.GetPropsCopy().Id;
            Name = name;
            Description = Description.DefaultValue;
            Priority = priority;
            Status = BackLogFeatureStatus.ToDo;
        }

        public object Clone()
        {
            return new ProjectBackLogFeatureProps(Id, Backlog, Name, Priority)
            {
                TechnicalScope = TechnicalScope,
                BusinessScope = BusinessScope,
                Description = Description,
                Priority = Priority,
                Status = Status
            };
        }
    }

    public class ProjectBackLogFeature : Entity<ProjectBackLogFeature, ProjectBackLogFeatureProps>
    {

        private ProjectBackLogFeature(ProjectBackLogFeatureProps props) : base(props)
        {
        }

        public static ProjectBackLogFeature Create(IdValueObject id, ProjectBacklog backlog, Name name, Priority priority)
        {
            var props = new ProjectBackLogFeatureProps(id, backlog, name, priority);

            return new ProjectBackLogFeature(props);
        }

        public void SetTechnicalScope(Description description   )
        {
            GetProps().TechnicalScope!.SetValue(description.GetValue());
        }

        public void SetBusinessScope(Description description)
        {
            GetProps().BusinessScope!.SetValue(description.GetValue());
        }

        public void UpdateName(Name name)
        {
            GetProps().Name.SetValue(name.GetValue());
        }

        public void UpdateDescription(Description description)
        {
            GetProps().Description.SetValue(description.GetValue());
        }

        public void UpdatePriority(Priority priority)
        {
            GetProps().Priority = priority;
        }

        public void ToDo()
        {
            GetProps().Status = BackLogFeatureStatus.ToDo;
        }

        public void InProgress()
        {
            GetProps().Status = BackLogFeatureStatus.InProgress;
        }

        public void OnHold()
        {
            GetProps().Status = BackLogFeatureStatus.OnHold;
        }

        public void Done()
        {
            GetProps().Status = BackLogFeatureStatus.Done;
        }

        public void Upstream()
        {
            GetProps().Status = BackLogFeatureStatus.Upstream;
        }

        public void Downstream()
        {
            GetProps().Status = BackLogFeatureStatus.Downstream;
        }

        public void SetComplexityLevel(ComplexityLevel complexityLevel)
        {
            GetProps().ComplexityLevel.SetValue<ComplexityLevel>(complexityLevel.Id);
        }
    }

    public class BackLogFeatureStatus : Enumeration
    {
        private BackLogFeatureStatus(int id, string name) : base(id, name)
        {
        }

        public static BackLogFeatureStatus ToDo = new BackLogFeatureStatus(1, "To Do");
        public static BackLogFeatureStatus InProgress = new BackLogFeatureStatus(2, "In Progress");
        public static BackLogFeatureStatus OnHold = new BackLogFeatureStatus(3, "On Hold");
        public static BackLogFeatureStatus Done = new BackLogFeatureStatus(4, "Done");
        public static BackLogFeatureStatus Upstream = new BackLogFeatureStatus(5, "In Upstream");
        public static BackLogFeatureStatus Downstream = new BackLogFeatureStatus(6, "In Downstream");
        public static BackLogFeatureStatus ToBeConfirmed = new BackLogFeatureStatus(7, "To Be Confirmed");
    }
}
