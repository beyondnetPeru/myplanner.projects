using MediatR;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCancel
{
    public record ProjectCancelCommand(string projectId) : IRequest<bool>;
}
