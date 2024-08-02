using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;
using MyPlanner.Shared.Domain.ValueObjects;

namespace MyPlanner.Projects.Domain
{
    public class ProjectScopeProps : IProps
    {
        public IdValueObject Id { get; set; }
        public IdValueObject ProjectId { get; private set; }
        public Project Project { get; private set; }
        public Description Description { get; private set; }
        public DateTimeUtcValueObject RegisterDate { get; private set; }

        public ProjectScopeProps(IdValueObject id, Project project, Description description, DateTimeUtcValueObject registerDate)
        {
            Id = id;
            ProjectId = project.GetPropsCopy().Id;
            Project = project;
            Description = description;
            RegisterDate = registerDate;
        }

        public object Clone()
        {
            return new ProjectScopeProps(Id, Project, Description, RegisterDate);
        }
    }

    public class ProjectScope : Entity<ProjectScope, ProjectScopeProps>
    {
        private ProjectScope(ProjectScopeProps props) : base(props)
        {
        }

        public static ProjectScope Create(IdValueObject id, Project project, Description description, DateTimeUtcValueObject registerDate)
        {
            var props = new ProjectScopeProps(id, project, description, registerDate);

            return new ProjectScope(props);
        }

        public void UpdateDescription(Description description)
        {
            GetProps().Description.SetValue(description.GetValue());
        }

        public void UpdateRegisterDate(DateTimeUtcValueObject registerDate)
        {
            GetProps().RegisterDate.SetValue(registerDate.GetValue());
        }
    }
}
