using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;
using MyPlanner.Shared.Domain.ValueObjects;

namespace MyPlanner.Projects.Domain
{
    public class ProjectBudgetProps : IProps
    {
        public IdValueObject Id { get; set; }
        public IdValueObject ProjectId { get; private set; }
        public Project Project { get; private set; }
        public Price Amount { get; private set; }
        public Description Description { get; private set; } = Description.DefaultValue;
        public DateTimeUtcValueObject RegisterDate { get; private set; }
        public StringValueObject ApprovedBy { get; private set; }

        public ProjectBudgetProps(IdValueObject id, Project project, Price amount, Description description, StringValueObject approvedBy)
        {
            Id = id;
            Project = project;
            ProjectId = project.GetPropsCopy().Id;
            Amount = amount;
            ApprovedBy = approvedBy;
            Description = Description.DefaultValue;
            RegisterDate = DateTimeUtcValueObject.Create(DateTime.UtcNow);
        }

        public object Clone()
        {
            return new ProjectBudgetProps(Id, Project, Amount, Description, ApprovedBy);
        }
    }
    public class ProjectBudget : Entity<ProjectBudget, ProjectBudgetProps>
    {
        public ProjectBudget(ProjectBudgetProps props) : base(props)
        {
        }

        public static ProjectBudget Create(IdValueObject id, Project project, Price amount, Description description, StringValueObject approvedBy)
        {
            return new ProjectBudget(new ProjectBudgetProps(id, project, amount, description, approvedBy));
        }
    }
}
