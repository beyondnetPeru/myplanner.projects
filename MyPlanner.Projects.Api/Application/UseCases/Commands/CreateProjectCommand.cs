using MediatR;
using MyPlanner.Projects.Infrastructure.Database.Tables;
using System.Runtime.Serialization;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands
{
    [DataContract]
    public class CreateProjectCommand : IRequest<bool>
    {
        [DataMember]
        public string UserId { get; private set; }
        
        [DataMember]
        public string Name { get; private set; }
        
        public CreateProjectCommand(string userId, string name)
        {
            Name = name;
            UserId = userId;
        }
    }
}
