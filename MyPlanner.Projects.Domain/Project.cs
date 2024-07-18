
using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;
using MyPlanner.Projects.Domain.DomainEvents;

namespace MyPlanner.Projects.Domain
{
    public class ProjectProps : IProps
    {

        public IdValueObject Id { get; set; }
        public Track? Track { get; set; }
        public Product? Product { get; set; }
        public Name Name { get; set; }
        public Description? Description { get; set; }
        public ProjectRiskLevel? RiskLevel { get; set; }
        public List<ProjectBacklog>? Backlogs { get; set; }
        public Price? Budget { get; set; }
        public List<Price>? ExtraBudget { get; set; }
        public List<ProjectScope>? Scopes { get; set; }
        public Owner? Owner { get; set; }
        public List<Stakeholder>? StakeHolders { get; set; }
        public ProjectStatus Status { get; set; }
        public Audit Audit { get; set; }

        public ProjectProps(IdValueObject id, Name name)
        {
            Id = id;
            Name = name;
            Status = ProjectStatus.NotStarted;
            Audit = Audit.Create("default");
        }

        public object Clone()
        {
            return new ProjectProps(Id, Name)
            {
                Track = Track,
                Product = Product,
                Description = Description,
                RiskLevel = RiskLevel,
                Backlogs = Backlogs,
                Budget = Budget,
                ExtraBudget = ExtraBudget,
                Scopes = Scopes,
                Owner = Owner,
                StakeHolders = StakeHolders,
                Status = Status,
                Audit = Audit
            };

        }
    }

    public class Project : Entity<Project, ProjectProps>
    {

        #region Constructors

        private Project(ProjectProps props) : base(props)
        {
            if (Props.Track!.IsNew)
                AddDomainEvent(new ProjectCreatedDomainEvent(props.Id.Value, props.Name.Value));
        }

        #endregion

        #region Factory Methods

        public static Project Create(IdValueObject id, Name name)
        {
            var props = new ProjectProps(id, name);

            var project = new Project(props);

            return project;
        }

        #endregion

        #region Methods

        public void UpdateTrack(Track track)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(track), "Project is completed, you can't update the track");

            Props.Track = track;

            Props.Audit.Update("default");
        }

        public void UpdateName(Name name)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(name), "Project is completed, you can't update the name");

            Props.Name = name;
            Props.Audit.Update("default");
        }

        public void UpdateDescription(Description description)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(description), "Project is completed, you can't update the description");

            Props.Description = description;
            Props.Audit.Update("default");
        }

        public void UpdateRiskLevel(ProjectRiskLevel riskLevel)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(riskLevel), "Project is completed, you can't update the risk level");

            Props.RiskLevel = riskLevel;
            Props.Audit.Update("default");

            AddDomainEvent(new ProjectRiskLevelUpdatedDomainEvent(GetPropsCopy().Id.Value, GetPropsCopy().Name.Value, GetPropsCopy().RiskLevel!.Name));
        }

        public void UpdateBudget(Price budget)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(budget), "Project is completed, you can't update the budget");

            Props.Budget = budget;
            Props.Audit.Update("default");
        }

        public void UpdateOwner(Owner owner)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(owner), "Project is completed, you can't update the owner");

            Props.Owner = owner;
            Props.Audit.Update("default");
        }

        public void Start()
        {
            if (GetPropsCopy().Status != ProjectStatus.NotStarted)
                AddBrokenRule("Status", "Project is already started");

            Props.Status = ProjectStatus.InProgress;
            Props.Audit.Update("default");

            AddDomainEvent(new ProjectStartedDomainEvent(GetPropsCopy().Id.Value, GetPropsCopy().Name.Value));
        }

        public void Complete()
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule("Status", "Project is already completed");

            if (GetPropsCopy().Status == ProjectStatus.OnHold)
                AddBrokenRule("Status", "Project is on hold, you can't complete it");

            Props.Status = ProjectStatus.Completed;
            Props.Audit.Update("default");

            AddDomainEvent(new ProjectCompletedDomainEvent(GetPropsCopy().Id.Value, GetPropsCopy().Name.Value));
        }

        public void Hold()
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule("Status", "Project is completed, you can't hold it");

            if (GetPropsCopy().Status == ProjectStatus.OnHold)
                AddBrokenRule("Status", "Project is already on hold");

            Props.Status = ProjectStatus.OnHold;
            Props.Audit.Update("default");

            AddDomainEvent(new ProjectHoldedDomainEvent(GetPropsCopy().Id.Value, GetPropsCopy().Name.Value));
        }

        public void AddBacklog(ProjectBacklog backlog)
        {
            if (GetPropsCopy().Backlogs!.Any(x => x.GetPropsCopy().Name.ToString()!.Equals(backlog.GetPropsCopy().Name.ToString(), StringComparison.OrdinalIgnoreCase)))
                return;

            Props.Backlogs!.Add(backlog);
            Props.Audit.Update("default");
        }

        public void RemoveBacklog(ProjectBacklog backlog)
        {
            if (GetPropsCopy().Backlogs!.Any(x => x.GetPropsCopy().Name.ToString()!.Equals(backlog.GetPropsCopy().Name.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                Props.Backlogs!.Remove(backlog);
                Props.Audit.Update("default");
            }
        }

        public void AddScope(ProjectScope scope)
        {
            if (GetPropsCopy().Scopes!.Any(x => x.Value.Description.ToString()!.Equals(scope.Value.Description.ToString(), StringComparison.OrdinalIgnoreCase)))
                return;

            Props.Scopes!.Add(scope);
            Props.Audit.Update("default");
        }

        public void RemoveScope(ProjectScope scope)
        {
            if (GetPropsCopy().Scopes!.Any(x => x.Value.Description.ToString().ToLower()!.Equals(scope.Value.Description.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                Props.Scopes!.Remove(scope);
                Props.Audit.Update("default");
            }
        }

        public void AddBudget(Price budget)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(budget), "Project is completed, you can't add budget");

            Props.Budget = budget;

            Props.Audit.Update("default");
        }

        public void AddExtraBudget(Price extraBudget)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(extraBudget), "Project is completed, you can't add extra budget");

            Props.ExtraBudget!.Add(extraBudget);

            Props.Audit.Update("default");
        }

        public void AddStakeholder(Stakeholder stakeholder)
        {
            if (GetPropsCopy().StakeHolders!.Any(x => x.Value.Name.ToString()!.Equals(stakeholder.Value.Name.ToString(), StringComparison.OrdinalIgnoreCase)))
                return;

            Props.StakeHolders!.Add(stakeholder);

            Props.Audit.Update("default");
        }

        public void RemoveStakeholder(Stakeholder stakeholder)
        {
            if (GetPropsCopy().StakeHolders!.Any(x => x.Value.Name.ToString().ToLower()!.Equals(stakeholder.Value.Name.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                Props.StakeHolders!.Remove(stakeholder);

                Props.Audit.Update("default");
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
