using AutoMapper;
using MediatR;
using MyPlanner.Projects.Api.Application.UseCases.Queries;

namespace MyPlanner.Projects.Api.Application.Endpoints
{
    public class ProjectServices(IMediator mediator, 
        IMapper mapper, 
        IProjectQueries queries, 
        ILogger<ProjectServices> logger)
    {
        public IMediator Mediator { get; set; } = mediator;
        public IMapper Mapper { get; set; } = mapper;
        public IProjectQueries Queries { get; set; } = queries;
        public ILogger<ProjectServices> Logger { get; set; } = logger;
    }
}
