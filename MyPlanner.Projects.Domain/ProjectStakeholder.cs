using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;


namespace MyPlanner.Projects.Domain
{
    public class ProjectStakeHolderProsp : IProps
    {
        public IdValueObject Id { get; set; }
        public Name Name { get; set; }
        public Description Description { get; set; }
        public StringValueObject Rol { get; set; }
        public Email Email { get; set; }
        public Project Project { get; set; }
        public IdValueObject ProjectId { get; set; }

        public ProjectStakeHolderProsp(IdValueObject id, Project project, Name name, Email email)
        {
            Id = id;
            Project = project;
            ProjectId = project.GetPropsCopy().Id;
            Name = name;
            Description = Description.Create(string.Empty);
            Rol = StringValueObject.Create(string.Empty);
            Email = email;
        }

        public object Clone()
        {
            return new ProjectStakeHolderProsp(Id, Project, Name, Email)
            {
                Description = Description,
                Rol = Rol

            };
        }
    }

    public class ProjectStakeholder : Entity<ProjectStakeholder, ProjectStakeHolderProsp>
    {
        private ProjectStakeholder(ProjectStakeHolderProsp props) : base(props)
        {
        }

        public static ProjectStakeholder Create(IdValueObject id, Project project, Name name, Email email)
        {
            var props = new ProjectStakeHolderProsp(id, project, name, email);

            return new ProjectStakeholder(props);
        }

        public void UpdateName(Name name)
        {
            GetProps().Name.SetValue(name.GetValue());
        }

        public void UpdateDescription(Description description)
        {
            GetProps().Description.SetValue(description.GetValue());
        }

        public void UpdateRol(StringValueObject rol)
        {
            GetProps().Rol.SetValue(rol.GetValue());
        }

        public void UpdateEmail(Email email)
        {
            GetProps().Email.SetValue(email.GetValue());
        }

    }
}
