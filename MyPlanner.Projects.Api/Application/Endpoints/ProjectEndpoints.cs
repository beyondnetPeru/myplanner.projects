using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyPlanner.EventBus.Extensions;
using MyPlanner.Projects.Api.Application.Dtos;
using MyPlanner.Projects.Api.Application.UseCases.Commands;
using MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCancel;
using MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateName;
using MyPlanner.Projects.Api.Application.UseCases.Queries;
using MyPlanner.Shared.Application.Dtos;

namespace MyPlanner.Projects.Api.Application.Endpoints
{
    public static class ProjectEndpoints
    {
        public static RouteGroupBuilder MapProjectApiV1(this IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("api/projects").HasApiVersion(1.0);

            api.MapGet("/projects", GetOrdersByUserAsync);
            api.MapGet("/projects/{id:string}", GetOrderByIdAsync);
            api.MapPost("/projects", CreateOrderAsync);
            api.MapPut("/projects/{id:string}", UpdateNameAsync);
            api.MapPost("/projects/{id:string}/cancel", CancelProjectAsync);


            return api;
        }

        public static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> CancelProjectAsync(
            [FromHeader(Name = "x-requestid")] Guid requestId,
            ProjectCancelCommand command, ProjectServices services)
        {
            if (requestId == Guid.Empty)
            {
                services.Logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", command);
                return TypedResults.BadRequest("RequestId is missing.");
            }

            var requestCancelProject = new IdentifiedCommand<ProjectCancelCommand, bool>(command, requestId);

            services.Logger.LogInformation("Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                                           requestCancelProject.GetGenericTypeName(),
                                           nameof(requestCancelProject.Id),
                                           requestCancelProject.Id,
                                           requestCancelProject);

            var commandResult = await services.Mediator.Send(requestCancelProject);

            if (!commandResult)
            {
                services.Logger.LogWarning("CancelProjectCommand failed - RequestId: {RequestId}", requestId);
                return TypedResults.Problem("Failed to cancel project.");
            }

            services.Logger.LogInformation("CancelProjectCommand succeeded - RequestId: {RequestId}", requestId);
            return TypedResults.Ok();
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

        public static async Task<Results<Ok, BadRequest<string>>> CreateOrderAsync(
        //[FromHeader(Name = "x-requestid")] Guid requestId,
        CreateProjectDto request,
        [AsParameters] ProjectServices services)
        {
            var requestId = Guid.NewGuid();

            //mask the credit card number

            services.Logger.LogInformation(
                "Sending command: {CommandName} - {IdProperty}: {CommandId}",
                request.GetGenericTypeName(),
                nameof(request.UserId),
                request.UserId); //don't log the request as it has CC number

            if (requestId == Guid.Empty)
            {
                services.Logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", request);
                return TypedResults.BadRequest("RequestId is missing.");
            }

            using (services.Logger.BeginScope(new List<KeyValuePair<string, object>> { new("IdentifiedCommandId", requestId) }))
            {
                var createProjectCommand = new ProjectCreateCommand(request.UserId, request.Name);

                var requestCreateProject = new IdentifiedCommand<ProjectCreateCommand, bool>(createProjectCommand, requestId);

                services.Logger.LogInformation(
                    "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    requestCreateProject.GetGenericTypeName(),
                    nameof(requestCreateProject.Id),
                    requestCreateProject.Id,
                    requestCreateProject);

                var result = await services.Mediator.Send(requestCreateProject);

                if (result)
                {
                    services.Logger.LogInformation("CreateProjectCommand succeeded - RequestId: {RequestId}", requestId);
                }
                else
                {
                    services.Logger.LogWarning("CreateProjectCommand failed - RequestId: {RequestId}", requestId);
                }

                return TypedResults.Ok();
            }
        }


        public static async Task<Results<Ok, BadRequest<string>>> UpdateNameAsync(
                       string id, ProjectUpdateNameDto request,
                       [FromHeader(Name = "x-requestid")] Guid requestId,
                       [AsParameters] ProjectServices services)
        {
            var changeProjectNameCommand = new ProjectUpdateNameCommand(id, request.NewName);

            services.Logger.LogInformation("Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                                           changeProjectNameCommand.GetGenericTypeName(),
                                           nameof(changeProjectNameCommand.Name),
                                           changeProjectNameCommand.Name,
                                           changeProjectNameCommand);

            var result = await services.Mediator.Send(changeProjectNameCommand);

            if (result)
            {
                services.Logger.LogInformation("ChangeProjectNameCommand succeeded - ProjectId: {ProjectId}", id);
            }
            else
            {
                services.Logger.LogWarning("ChangeProjectNameCommand failed - ProjectId: {ProjectId}", id);
            }

            return TypedResults.Ok();
        }
    }

    public record CreateProjectRequest(
    string UserId,
    string Name);
}
