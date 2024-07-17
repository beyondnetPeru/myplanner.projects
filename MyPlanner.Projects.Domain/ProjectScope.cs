using BeyondNet.Ddd;

namespace MyPlanner.Projects.Domain
{
    public struct ProjectScopeProps
    {
        public string Description { get; set; }
        public DateTime RegisterDate { get; set; }
    }

    public class ProjectScope : ValueObject<ProjectScopeProps>
    {
        public ProjectScope(ProjectScopeProps value) : base(value)
        {
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value.Description + Value.RegisterDate.ToString();
        }
    }
}
