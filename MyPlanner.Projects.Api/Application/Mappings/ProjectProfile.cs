using AutoMapper;
using MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCreate;
using MyPlanner.Projects.Api.Application.UseCases.Queries;

namespace MyPlanner.Projects.Api.Application.Mappings
{
    public class ProjectProfile : Profile
    {
        public void ConfigureProfile(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ProjectDto, ProjectInfo>();
            configuration.CreateMap<ProjectCreateDto, ProjectCreateCommand>();
        }
    }
}
