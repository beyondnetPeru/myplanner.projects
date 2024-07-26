using MediatR;
using MyPlanner.Projects.Infrastructure.Database.Tables;
using System.Runtime.Serialization;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCreate
{
    [DataContract]
    public class ProjectCreateCommand : IRequest<bool>
    {
        [DataMember]
        public string UserId { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        public ProjectCreateCommand(string userId, string name)
        {
            Name = name;
            UserId = userId;
        }
    }
}
