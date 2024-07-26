using MediatR;

namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateTrack
{
    public class ProjectUpdateTrackCommand : IRequest<bool>
    {
        public string ProjectId { get; }
        public string Track { get; }

        public ProjectUpdateTrackCommand(string projectId, string track)
        {
            ProjectId = projectId;
            Track = track;
        }
    }
}
