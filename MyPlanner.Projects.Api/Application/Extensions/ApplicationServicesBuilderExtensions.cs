
using Microsoft.EntityFrameworkCore;
using MyPlanner.IntegrationEventLogEF.Services;
using MyPlanner.Projects.Api.Application.Services;
using MyPlanner.Projects.Api.Application.UseCases.Queries;
using MyPlanner.Projects.Domain;
using MyPlanner.Shared.Infrastructure.Database;
using MyPlanner.Shared.Infrastructure.Idempotency;
using MyPlanner.Projects.Infrastructure.Repositories;
using MyPlanner.Shared.Application.Behaviors;
using MyPlanner.Shared.Api.Application.Extensions;
using MyPlanner.Projects.Api.Application.Extensions;
using MyPlanner.Projects.Infrastructure.Database;

namespace MyPlanner.Shared.Api.Application.Extensions
{
    public static partial class ApplicationServicesBuilderExtensions
    {
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            var services = builder.Services;

            // Add the authentication services to DI
            //builder.AddDefaultAuthentication();

            services.AddDbContext<ProjectDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Add the integration services that consume the DbContext
            services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService<ProjectDbContext>>();

            services.AddTransient<IProjectIntegrationEventService, ProjectIntegrationEventService>();

            //builder.AddRabbitMqEventBus("eventbus")
            //       .AddEventBusSubscriptions();

            services.AddHttpContextAccessor();
            //services.AddTransient<IIdentityService, IdentityService>();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining(typeof(Program));

                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
            });

            services.AddAutoMapper(typeof(Program).Assembly);

            // Register the command validators for the validator behavior (validators based on FluentValidation library)
            //services.AddSingleton<IValidator<CancelOrderCommand>, CancelOrderCommandValidator>();

            services.AddScoped<IProjectQueries, ProjectQueries>();
            services.AddScoped<IProjectRepository, ProjectRepository>();

            // Idempotency Service
            services.AddScoped<IRequestManager, Projects.Infrastructure.Idempotency.RequestManager>();
        }

        private static void AddEventBusSubscriptions(this IEventBusBuilder eventBus)
        {
            //eventBus.AddSubscription<IntegrationEvent, IntegrationEventHandler>();
        }
    }
}
