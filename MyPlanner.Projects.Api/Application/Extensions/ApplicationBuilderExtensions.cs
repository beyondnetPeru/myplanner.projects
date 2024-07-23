using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyPlanner.IntegrationEventLogEF.Services;
using MyPlanner.Projects.Api.Application.Services;
using MyPlanner.Projects.Api.Application.UseCases.Queries;
using MyPlanner.Projects.Domain;
using MyPlanner.Projects.Infrastructure.Database;
using MyPlanner.Projects.Infrastructure.Repositories;
using MyPlanner.Shared.Application.Behaviors;

namespace MyPlanner.Projects.Api.Application.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            var services = builder.Services;

            services.AddDbContext<ProjectDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService<ProjectDbContext>>();

            services.AddTransient<IProjectIntegrationEventService, ProjectIntegrationEventService>();
         
            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(Program).Assembly);

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining(typeof(Program));

                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
            });

            // Register the command validators for the validator behavior (validators based on FluentValidation library)
            //services.AddSingleton<IValidator<CancelOrderCommand>, CancelOrderCommandValidator>();

            services.AddScoped<IProjectQueries, ProjectQueries>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
        }

        private static void AddEventBusSubscriptions(this IEventBusBuilder eventBus)
        {
            //eventBus.AddSubscription<IntegrationEvent, IntegrationEventHandler>();
        }

        public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
        {
            // Enable Semantic Kernel OpenTelemetry
            AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);

            builder.AddBasicServiceDefaults();

            return builder;
        }

        /// <summary>
        /// Adds the services except for making outgoing HTTP calls.
        /// </summary>
        /// <remarks>
        /// This allows for things like Polly to be trimmed out of the app if it isn't used.
        /// </remarks>
        public static IHostApplicationBuilder AddBasicServiceDefaults(this IHostApplicationBuilder builder)
        {
            // Default health checks assume the event bus and self health checks
            builder.AddDefaultHealthChecks();

            return builder;
        }


        public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                // Add a default liveness check to ensure app is responsive
                .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

            return builder;
        }

        public static WebApplication MapDefaultEndpoints(this WebApplication app)
        {
            // Uncomment the following line to enable the Prometheus endpoint (requires the OpenTelemetry.Exporter.Prometheus.AspNetCore package)
            // app.MapPrometheusScrapingEndpoint();

            // Adding health checks endpoints to applications in non-development environments has security implications.
            // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
            if (app.Environment.IsDevelopment())
            {
                // All health checks must pass for app to be considered ready to accept traffic after starting
                app.MapHealthChecks("/health");

                // Only health checks tagged with the "live" tag must pass for app to be considered alive
                app.MapHealthChecks("/alive", new HealthCheckOptions
                {
                    Predicate = r => r.Tags.Contains("live")
                });
            }

            return app;
        }
    }
}
