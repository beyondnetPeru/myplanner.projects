using MediatR;
using MyPlanner.Projects.Domain;
using MyPlanner.Shared.Infrastructure.Idempotency;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCancel
{
    public class ProjectCancelCommandHandler : IRequestHandler<ProjectCancelCommand, bool>
    {
        private readonly IProjectRepository projectRepository;

        public ProjectCancelCommandHandler(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public async Task<bool> Handle(ProjectCancelCommand request, CancellationToken cancellationToken)
        {
            var projectToUpdate = await projectRepository.GetAsync(request.ProjectId);

            if (projectToUpdate == null)
            {
                return false;
            }

            projectToUpdate.Cancel();

            if (!projectToUpdate.IsValid)
            {
                return false;
            }

            return await projectRepository.UnitOfWork.SaveEntitiesAsync(projectToUpdate, cancellationToken);
        }
    }

    public class CancelProjectIdentifiedCommandHandler : IdentifiedCommandHandler<ProjectCancelCommand, bool>
    {
        public CancelProjectIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager, ILogger<IdentifiedCommandHandler<ProjectCancelCommand, bool>> logger) : base(mediator, requestManager, logger)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;
        }
    }
}
