using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;
using MyPlanner.Shared.Domain.ValueObjects;

namespace MyPlanner.Projects.Domain
{
    public class ProjectTrackProps : IProps
    {
        public IdValueObject Id { get; private set; }
        public Project Project { get; private set; }
        public Name Name { get; private set; }
        public Description? Description { get; private set; }

        public ProjectTrackProps(IdValueObject id, Project project, Name name)
        {
            Id = id;
            Name = name;
            Project = project;
            Description = Description.DefaultValue;
        }

        public object Clone()
        {
            return new ProjectTrackProps(Id, Project, Name)
            {
                Description = Description
            };
        }
    }

    public class ProjectFeatureTrack : Entity<ProjectFeatureTrack, ProjectTrackProps>
    {
        public ProjectFeatureTrack(ProjectTrackProps props) : base(props)
        {
        }

        public static ProjectFeatureTrack Create(IdValueObject id, Project project, Name name)
        {
            var props = new ProjectTrackProps(id, project, name);

            return new ProjectFeatureTrack(props);
        }

        public void UpdateName(Name name)
        {
            GetProps().Name.SetValue(name.GetValue());
        }

    }
}
