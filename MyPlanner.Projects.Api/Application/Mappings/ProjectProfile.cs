using AutoMapper;
using MyPlanner.Projects.Api.Application.Dtos;
using MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCreate;
using MyPlanner.Projects.Api.Application.UseCases.Queries;
using MyPlanner.Projects.Infrastructure.Database.Tables;

namespace MyPlanner.Projects.Api.Application.Mappings
{
    public class ProjectProfile : Profile
    {
        public void ConfigureProfile(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ProjectDto, ProjectInfo>();
            configuration.CreateMap<CreateProjectDto, ProjectCreateCommand>();
        }
    }
}
