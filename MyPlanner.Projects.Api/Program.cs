using MyPlanner.Projects.Api.Application.Endpoints;
using MyPlanner.Projects.Api.Application.Extensions;
using MyPlanner.Shared.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddProblemDetails();

var withApiVersioning = builder.Services.AddApiVersioning();

builder.AddDefaultOpenApi(withApiVersioning);

var app = builder.Build();

app.MapDefaultEndpoints();

var projects = app.NewVersionedApi("Projects");

projects.MapProjectApiV1();
         //.RequireAuthorization(); 

app.UseDefaultOpenApi();
app.Run();