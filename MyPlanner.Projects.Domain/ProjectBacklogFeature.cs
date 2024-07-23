
using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;

namespace MyPlanner.Projects.Domain
{
    public class ProjectBackLogFeatureProps : IProps
    {
        public IdValueObject Id { get; private set; }
        public Name Name { get; private set; }
        public ProjectBacklog Backlog { get; private set; }
        public IdValueObject BacklogId { get; private set; }
        public Description Description { get; private set; }
        public Priority Priority { get; set; }
        public BackLogFeatureStatus Status { get; set; }

        public ProjectBackLogFeatureProps(IdValueObject id,  ProjectBacklog backlog, Name name, Priority priority)
        {
            Id = id;
            Backlog = backlog;
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

        public static ProjectBackLogFeature Create(ProjectBacklog backlog, IdValueObject id, Name name, Priority priority)
        {
            var props = new ProjectBackLogFeatureProps(id, backlog, name, priority);

            return new ProjectBackLogFeature(props);
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
    }
}
