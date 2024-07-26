namespace MyPlanner.Projects.Api.Application.UseCases.Commands.ProjectUpdateTrack
{
    public class ProjectUpdateTrackDto
    {
        public string ProjectId { get; }
        public string Track { get; }

        public ProjectUpdateTrackDto(string projectId, string track)
        {
            ProjectId = projectId;
            Track = track;
        }
    }
}
