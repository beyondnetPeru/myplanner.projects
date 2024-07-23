using AutoMapper;
using MyPlanner.Projects.Api.Application.UseCases.Queries;
using MyPlanner.Projects.Infrastructure.Database.Tables;

namespace MyPlanner.Projects.Api.Application.Mappings
{
    public class ProjectProfile : Profile
    {
        public void ConfigureProfile(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ProjectTable, ProjectInfo>();
        }
    }
}
