
using BeyondNet.Ddd;
using MyPlanner.Projects.Domain.DomainEvents;

namespace MyPlanner.Projects.Domain
{

    public class Project : Entity<Project>
    {
        #region Properties

        public Track? Track { get; private set; }
        public Product Product { get; private set; } = Product.Default;
        public Name Name { get; private set; }
        public Description? Description { get; private set; }
        public ProjectRiskLevel RiskLevel { get; private set; } = ProjectRiskLevel.Low;
        public List<ProjectBacklog> Backlogs { get; private set; } = new List<ProjectBacklog>();
        public Price? Budget { get; private set; }
        public List<Price> ExtraBudget { get; private set; } = new List<Price>();
        public List<ProjectScope> Scopes { get; private set; } = new List<ProjectScope>();
        public Owner? Owner { get; private set; }
        public List<Stakeholder> StakeHolders { get; private set; } = new List<Stakeholder>();
        public ProjectStatus Status { get; private set; }
        public Audit Audit { get; set; }

        #endregion

        #region Constructors

        private Project(Name name, Description description)
        {
            Name = name;
            Description = description;
            Status = ProjectStatus.NotStarted;
            Audit = Audit.Create("default");

            AddDomainEvent(new ProjectCreatedDomainEvent(Id.Value, Name.Value));
        }

        #endregion

        #region Factory Methods

        public static Project Create(Name name, Description description)
        {
            return new Project(name, description);
        }

        #endregion

        #region Methods

        public void UpdateTrack(Track track)
        {
            if (Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(track), "Project is completed, you can't update the track");

            Track = track;
            Audit.Update("default");
        }

        public void UpdateName(Name name)
        {
            if (Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(name), "Project is completed, you can't update the name");

            Name = name;
            Audit.Update("default");
        }

        public void UpdateDescription(Description description)
        {
            if (Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(description), "Project is completed, you can't update the description");

            Description = description;
            Audit.Update("default");
        }

        public void UpdateRiskLevel(ProjectRiskLevel riskLevel)
        {
            if (Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(riskLevel), "Project is completed, you can't update the risk level");

            RiskLevel = riskLevel;
            Audit.Update("default");

            AddDomainEvent(new ProjectRiskLevelUpdatedDomainEvent(Id.Value, Name.Value, RiskLevel.Name));
        }

        public void UpdateBudget(Price budget)
        {
            if (Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(budget), "Project is completed, you can't update the budget");

            Budget = budget;
            Audit.Update("default");
        }

        public void UpdateOwner(Owner owner)
        {
            if (Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(owner), "Project is completed, you can't update the owner");

            Owner = owner;
            Audit.Update("default");
        }

        public void Start()
        {
            if (Status != ProjectStatus.NotStarted)
                AddBrokenRule(nameof(Status), "Project is already started");

            Status = ProjectStatus.InProgress;
            Audit.Update("default");

            AddDomainEvent(new ProjectStartedDomainEvent(Id.Value, Name.Value));
        }

        public void Complete()
        {
            if (Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(Status), "Project is already completed");

            if (Status == ProjectStatus.OnHold)
                AddBrokenRule(nameof(Status), "Project is on hold, you can't complete it");

            Status = ProjectStatus.Completed;
            Audit.Update("default");

            AddDomainEvent(new ProjectCompletedDomainEvent(Id.Value, Name.Value));
        }

        public void Hold()
        {
            if (Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(Status), "Project is completed, you can't hold it");

            if (Status == ProjectStatus.OnHold)
                AddBrokenRule(nameof(Status), "Project is already on hold");

            Status = ProjectStatus.OnHold;
            Audit.Update("default");

            AddDomainEvent(new ProjectHoldedDomainEvent(Id.Value, Name.Value));
        }

        public void AddBacklog(ProjectBacklog backlog)
        {
            if (Backlogs.Any(x => x.Id == backlog.Id) ||
                Backlogs.Any(x => x.Name.ToString()!.Equals(backlog.Name.ToString(), StringComparison.OrdinalIgnoreCase)))
                return;

            Backlogs.Add(backlog);
            Audit.Update("default");
        }

        public void RemoveBacklog(ProjectBacklog backlog)
        {
            if (Backlogs.Any(x => x.Id == backlog.Id))
                Backlogs.Remove(backlog);

            Audit.Update("default");
        }

        public void AddScope(ProjectScope scope)
        {
            if (Scopes.Any(x => x.Value.Description.ToString()!.Equals(scope.Value.Description.ToString(), StringComparison.OrdinalIgnoreCase)))
                return;

            Scopes.Add(scope);
            Audit.Update("default");
        }

        public void RemoveScope(ProjectScope scope)
        {
            if (Scopes.Any(x => x.Value.Description.ToString().ToLower()!.Equals(scope.Value.Description.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                Scopes.Remove(scope);
                Audit.Update("default");
            }
        }

        public void AddBudget(Price budget)
        {
            if (Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(budget), "Project is completed, you can't add budget");

            Budget = budget;

            Audit.Update("default");
        }

        public void AddExtraBudget(Price extraBudget)
        {
            if (Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(extraBudget), "Project is completed, you can't add extra budget");

            ExtraBudget.Add(extraBudget);

            Audit.Update("default");
        }

        public void AddStakeholder(Stakeholder stakeholder)
        {
            if (StakeHolders.Any(x => x.Value.Name.ToString()!.Equals(stakeholder.Value.Name.ToString(), StringComparison.OrdinalIgnoreCase)))
                return;

            StakeHolders.Add(stakeholder);

            Audit.Update("default");
        }

        public void RemoveStakeholder(Stakeholder stakeholder)
        {
            if (StakeHolders.Any(x => x.Value.Name.ToString().ToLower()!.Equals(stakeholder.Value.Name.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                StakeHolders.Remove(stakeholder);

                Audit.Update("default");
            }

        }

        #endregion
    }

    #region Status

    public class ProjectStatus : Enumeration
    {
        public static ProjectStatus NotStarted = new ProjectStatus(1, nameof(NotStarted));
        public static ProjectStatus InProgress = new ProjectStatus(2, nameof(InProgress));
        public static ProjectStatus Completed = new ProjectStatus(3, nameof(Completed));
        public static ProjectStatus OnHold = new ProjectStatus(4, nameof(OnHold));

        private ProjectStatus(int id, string name) : base(id, name)
        {
        }
    }

    #endregion
}
