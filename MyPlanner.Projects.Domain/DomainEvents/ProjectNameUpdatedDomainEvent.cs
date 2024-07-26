using BeyondNet.Ddd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPlanner.Projects.Domain.DomainEvents
{
    public class ProjectNameUpdatedDomainEvent : DomainEvent
    {
        public string ProjectId { get; }
        public string ProjectName { get; }

        public ProjectNameUpdatedDomainEvent(string projectId, string projectName)
        {
            ProjectId = projectId;
            ProjectName = projectName;
        }
    }
}
