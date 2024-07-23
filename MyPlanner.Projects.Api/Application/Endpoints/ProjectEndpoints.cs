using Microsoft.AspNetCore.Http.HttpResults;
using MyPlanner.Projects.Api.Application.UseCases.Queries;
using MyPlanner.Shared.Application.Dtos;

namespace MyPlanner.Projects.Api.Application.Endpoints
{
    public static class ProjectEndpoints
    {
        public static RouteGroupBuilder MapProjectApiV1(this IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("api/projects").HasApiVersion(1.0);

            api.MapGet("/orders", GetOrdersByUserAsync);


            return api;
        }

        public static async Task<Ok<IEnumerable<ProjectInfo>>> GetOrdersByUserAsync([AsParameters] ProjectServices services)
        {
            var orders = await services.Queries.GetProjectsAsync(new PaginationDto() { Page = 1, RecordsPerPage = 10 });
            return TypedResults.Ok(orders);
        }

        public static async Task<Ok<ProjectInfo>> GetOrderByIdAsync(string id, [AsParameters] ProjectServices services)
        {
            var order = await services.Queries.GetProjectAsync(id);
            return TypedResults.Ok(order);
        }
    }
}
