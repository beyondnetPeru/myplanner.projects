
using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;

namespace MyPlanner.Projects.Domain
{
    public class ProjectBackLogFeatureProps : IProps
    {
        public Name Name { get; set; }
        public Description? Description { get; set; }
        public Priority? Priority { get; set; }
        public BackLogFeatureStatus Status { get; set; }

        public ProjectBackLogFeatureProps(Name name)
        {
            Name = name;
            Description = Description.DefaultValue;
            Priority = Priority.DefaultValue;
            Status = BackLogFeatureStatus.ToDo;
        }

        public object Clone()
        {
            return new ProjectBackLogFeatureProps(Name)
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

        public static ProjectBackLogFeature Create(Name name)
        {
            var props = new ProjectBackLogFeatureProps(name);

            return new ProjectBackLogFeature(props);
        }

        public void UpdateName(Name name)
        {
            Props.Name = name;
        }

        public void UpdateDescription(Description description)
        {
            Props.Description = description;
        }

        public void UpdatePriority(Priority priority)
        {
            Props.Priority = priority;
        }

        public void ToDo()
        {
            Props.Status = BackLogFeatureStatus.ToDo;
        }

        public void InProgress()
        {
            Props.Status = BackLogFeatureStatus.InProgress;
        }

        public void OnHold()
        {
            Props.Status = BackLogFeatureStatus.OnHold;
        }

        public void Done()
        {
            Props.Status = BackLogFeatureStatus.Done;
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
