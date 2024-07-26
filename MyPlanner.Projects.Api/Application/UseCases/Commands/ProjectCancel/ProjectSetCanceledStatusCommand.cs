using MediatR;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectCancel
{
   public record ProjectSetCanceledStatusCommand(string projectId) : IRequest<bool>;
}
