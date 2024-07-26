using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyPlanner.EventBus.Extensions;
using MyPlanner.Projects.Api.Application.Dtos;
using MyPlanner.Projects.Api.Application.UseCases.Commands;
using MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCancel;
using MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectComplete;
using MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCreate;
using MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectOnHold;
using MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectResume;
using MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectStart;
using MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateDescription;
using MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateName;
using MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateOwner;
using MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateRiskLevel;
using MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateTrack;
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
            api.MapPut("/projects/{id:string}/updatename", UpdateNameAsync);
            api.MapPut("/projects/{id:string}/updatedescription", UpdateDescriptionAsync);
            api.MapPut("/projects/{id:string}/updatetrack", UpdateTrackAsync);
            api.MapPut("/projects/{id:string}/updaterisklevel", UpdateRiskLevelAsync);
            api.MapPut("/projects/{id:string}/updateowner", UpdateOwnerAsync);
            api.MapPost("/projects/{id:string}/start", StartProjectAsync);
            api.MapPost("/projects/{id:string}/onhold", OnHoldProjectAsync);
            api.MapPost("/projects/{id:string}/resume", ResumeProjectAsync);
            api.MapPost("/projects/{id:string}/complete", CompleteProjectAsync);
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

        public static async Task<Results<Ok, BadRequest<string>>> UpdateDescriptionAsync(
                       string id, ProjectUpdateDescriptionDto request,
                       [FromHeader(Name = "x-requestid")] Guid requestId,
                       [AsParameters] ProjectServices services)
        {
            var changeProjectDescriptionCommand = new ProjectUpdateDescriptionCommand(id, request.Description);

            services.Logger.LogInformation("Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                               changeProjectDescriptionCommand.GetGenericTypeName(),
                               nameof(changeProjectDescriptionCommand.Description),
                               changeProjectDescriptionCommand.Description,
                               changeProjectDescriptionCommand);

            var result = await services.Mediator.Send(changeProjectDescriptionCommand);

            if (result)
            {
                services.Logger.LogInformation("ChangeProjectDescriptionCommand succeeded - ProjectId: {ProjectId}", id);
            }
            else
            {
                services.Logger.LogWarning("ChangeProjectDescriptionCommand failed - ProjectId: {ProjectId}", id);
            }

            return TypedResults.Ok();
        }

        public static async Task<Results<Ok, BadRequest<string>>> UpdateTrackAsync(
                       string id, ProjectUpdateTrackDto request,
                       [FromHeader(Name = "x-requestid")] Guid requestId,
                       [AsParameters] ProjectServices services)
        {
            var changeProjectTrackCommand = new ProjectUpdateTrackCommand(id, request.Track);

            services.Logger.LogInformation("Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                                              changeProjectTrackCommand.GetGenericTypeName(),
                                              nameof(changeProjectTrackCommand.Track),
                                              changeProjectTrackCommand.Track,
                                              changeProjectTrackCommand);

            var result = await services.Mediator.Send(changeProjectTrackCommand);

            if (result)
            {
                services.Logger.LogInformation("ChangeProjectTrackCommand succeeded - ProjectId: {ProjectId}", id);
            }
            else
            {
                services.Logger.LogWarning("ChangeProjectTrackCommand failed - ProjectId: {ProjectId}", id);
            }

            return TypedResults.Ok();
        }

        public static async Task<Results<Ok, BadRequest<string>>> UpdateRiskLevelAsync(
                       string id, ProjectUpdateRiskLevelDto request,
                       [FromHeader(Name = "x-requestid")] Guid requestId,
                       [AsParameters] ProjectServices services)
        {
            var changeProjectRiskLevelCommand = new ProjectUpdateRiskLevelCommand(id, request.RiskLevel);

            services.Logger.LogInformation("Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                                                             changeProjectRiskLevelCommand.GetGenericTypeName(),
                                                             nameof(changeProjectRiskLevelCommand.RiskLevel),
                                                             changeProjectRiskLevelCommand.RiskLevel,
                                                             changeProjectRiskLevelCommand);

            var result = await services.Mediator.Send(changeProjectRiskLevelCommand);

            if (result)
            {
                services.Logger.LogInformation("ChangeProjectRiskLevelCommand succeeded - ProjectId: {ProjectId}", id);
            }
            else
            {
                services.Logger.LogWarning("ChangeProjectRiskLevelCommand failed - ProjectId: {ProjectId}", id);
            }

            return TypedResults.Ok();
        }

        public static async Task<Results<Ok, BadRequest<string>>> UpdateOwnerAsync(
                       string id, ProjectUpdateOwnerDto request,
                       [FromHeader(Name = "x-requestid")] Guid requestId,
                       [AsParameters] ProjectServices services)
        {
            var changeProjectOwnerCommand = new ProjectUpdateOwnerCommand(id, request.Owner);

            services.Logger.LogInformation("Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                                            changeProjectOwnerCommand.GetGenericTypeName(),
                                            nameof(changeProjectOwnerCommand.Owner),
                                            changeProjectOwnerCommand.Owner,
                                                                                                                                                                                                                                                                changeProjectOwnerCommand);

            var result = await services.Mediator.Send(changeProjectOwnerCommand);

            if (result)
            {
                services.Logger.LogInformation("ChangeProjectOwnerCommand succeeded - ProjectId: {ProjectId}", id);
            }
            else
            {
                services.Logger.LogWarning("ChangeProjectOwnerCommand failed - ProjectId: {ProjectId}", id);
            }

            return TypedResults.Ok();
        }

        public static async Task<Results<Ok, BadRequest<string>>> StartProjectAsync(
                       string id,
                       [FromHeader(Name = "x-requestid")] Guid requestId,
                       [AsParameters] ProjectServices services)
        {
            var startProjectCommand = new ProjectStartCommand(id);

            services.Logger.LogInformation("Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                                            startProjectCommand.GetGenericTypeName(),
                                            nameof(startProjectCommand.ProjectId),
                                            startProjectCommand.ProjectId,
                                            startProjectCommand);

            var result = await services.Mediator.Send(startProjectCommand);

            if (result)
            {
                services.Logger.LogInformation("StartProjectCommand succeeded - ProjectId: {ProjectId}", id);
            }
            else
            {
                services.Logger.LogWarning("StartProjectCommand failed - ProjectId: {ProjectId}", id);
            }

            return TypedResults.Ok();
        }

        public static async Task<Results<Ok, BadRequest<string>>> OnHoldProjectAsync(
                       string id,
                       [FromHeader(Name = "x-requestid")] Guid requestId,
                       [AsParameters] ProjectServices services)
        {
            var onHoldProjectCommand = new ProjectOnHoldCommand(id);

            services.Logger.LogInformation("Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                                 onHoldProjectCommand.GetGenericTypeName(),
                                 nameof(onHoldProjectCommand.ProjectId),
                                 onHoldProjectCommand.ProjectId,
                                 onHoldProjectCommand);

            var result = await services.Mediator.Send(onHoldProjectCommand);

            if (result)
            {
                services.Logger.LogInformation("OnHoldProjectCommand succeeded - ProjectId: {ProjectId}", id);
            }
            else
            {
                services.Logger.LogWarning("OnHoldProjectCommand failed - ProjectId: {ProjectId}", id);
            }

            return TypedResults.Ok();
        }

        public static async Task<Results<Ok, BadRequest<string>>> ResumeProjectAsync(
                        string id,[FromHeader(Name = "x-requestid")] Guid requestId,
                        [AsParameters] ProjectServices services)
        {
            var resumeProjectCommand = new ProjectResumeCommand(id);

            services.Logger.LogInformation("Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                                    resumeProjectCommand.GetGenericTypeName(),
                                    nameof(resumeProjectCommand.ProjectId),
                                    resumeProjectCommand.ProjectId,
                                    resumeProjectCommand);

            var result = await services.Mediator.Send(resumeProjectCommand);

            if (result)
            {
                services.Logger.LogInformation("ResumeProjectCommand succeeded - ProjectId: {ProjectId}", id);
            }
            else
            {
                services.Logger.LogWarning("ResumeProjectCommand failed - ProjectId: {ProjectId}", id);
            }

            return TypedResults.Ok();
        }

        public static async Task<Results<Ok, BadRequest<string>>> CompleteProjectAsync(
                        string id,
                        [FromHeader(Name = "x-requestid")] Guid requestId,
                        [AsParameters] ProjectServices services)
        {
            var completeProjectCommand = new ProjectCompleteCommand(id);

            services.Logger.LogInformation("Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                                                   completeProjectCommand.GetGenericTypeName(),
                                                   nameof(completeProjectCommand.ProjectId),
                                                   completeProjectCommand.ProjectId,
                                                   completeProjectCommand);

            var result = await services.Mediator.Send(completeProjectCommand);

            if (result)
            {
                services.Logger.LogInformation("CompleteProjectCommand succeeded - ProjectId: {ProjectId}", id);
            }
            else
            {
                services.Logger.LogWarning("CompleteProjectCommand failed - ProjectId: {ProjectId}", id);
            }

            return TypedResults.Ok();
        }
    }

    public record CreateProjectRequest(
    string UserId,
    string Name);
}
