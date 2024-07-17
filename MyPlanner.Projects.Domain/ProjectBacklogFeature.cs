
using BeyondNet.Ddd;
using BeyondNet.Ddd.ValueObjects;
using MyPlanner.Projects.Domain;

namespace MyPlanner.Projects.Domain
{
    public class ProjectBackLogFeature : Entity<ProjectBackLogFeature>
    {
        public Name Name { get; set; }
        public Description Description { get; set; }
        public Priority Priority { get; set; }
        public BackLogFeatureStatus Status { get; set; }

        private ProjectBackLogFeature(Name name)
        {
            Name = name;
            Description = Description.DefaultValue;
            Priority = Priority.DefaultValue;
            Status = BackLogFeatureStatus.ToDo;

        }

        public static ProjectBackLogFeature Create(Name name)
        {
            return new ProjectBackLogFeature(name);
        }

        public void UpdateName(Name name)
        {
            Name = name;
        }

        public void UpdateDescription(Description description)
        {
            Description = description;
        }

        public void UpdatePriority(Priority priority)
        {
            Priority = priority;
        }

        public void ToDo()
        {
            Status = BackLogFeatureStatus.ToDo;
        }

        public void InProgress()
        {
            Status = BackLogFeatureStatus.InProgress;
        }

        public void OnHold()
        {
            Status = BackLogFeatureStatus.OnHold;
        }

        public void Done()
        {
            Status = BackLogFeatureStatus.Done;
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
