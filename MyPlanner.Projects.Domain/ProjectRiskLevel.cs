

using BeyondNet.Ddd;

namespace MyPlanner.Projects.Domain
{
    public class ProjectRiskLevel : Enumeration
    {
        public static ProjectRiskLevel Low = new ProjectRiskLevel(1, "Low");
        public static ProjectRiskLevel Medium = new ProjectRiskLevel(2, "Medium");
        public static ProjectRiskLevel High = new ProjectRiskLevel(3, "High");

        public ProjectRiskLevel(int id, string name) : base(id, name)
        {
        }
     }
}
