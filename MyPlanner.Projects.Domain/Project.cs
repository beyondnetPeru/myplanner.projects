
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
            if (this.IsNew())
                AddDomainEvent(new ProjectCreatedDomainEvent(props.Id.GetValue(), props.Name.GetValue()));
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

            var props = GetProps();

            props.Track = track;
            props.Audit.Update("default");

            SetProps(props);
        }

        public void UpdateName(Name name)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(name), "Project is completed, you can't update the name");

            var props = GetProps();
            
            props.Name = name;
            props.Audit.Update("default");
            
            SetProps(props);
        }

        public void UpdateDescription(Description description)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(description), "Project is completed, you can't update the description");

            var props = GetProps();
            
            props.Description = description;
            props.Audit.Update("default");

            SetProps(props);
        }

        public void UpdateRiskLevel(ProjectRiskLevel riskLevel)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(riskLevel), "Project is completed, you can't update the risk level");

            var props = GetProps();

            props.RiskLevel = riskLevel;
            props.Audit.Update("default");

            SetProps(props);

            AddDomainEvent(new ProjectRiskLevelUpdatedDomainEvent(GetPropsCopy().Id.GetValue(), GetPropsCopy().Name.GetValue(), GetPropsCopy().RiskLevel!.Name));
        }

        public void UpdateBudget(Price budget)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(budget), "Project is completed, you can't update the budget");

            var props = GetProps();

            props.Budget = budget;
            props.Audit.Update("default");

            SetProps(props);
        }

        public void UpdateOwner(Owner owner)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(owner), "Project is completed, you can't update the owner");

            var props = GetProps();

            props.Owner = owner;
            props.Audit.Update("default");

            SetProps(props);
        }

        public void Start()
        {
            if (GetPropsCopy().Status != ProjectStatus.NotStarted)
                AddBrokenRule("Status", "Project is already started");

            var props = GetProps();

            props.Status = ProjectStatus.InProgress;
            props.Audit.Update("default");

            SetProps(props);

            AddDomainEvent(new ProjectStartedDomainEvent(GetPropsCopy().Id.GetValue(), GetPropsCopy().Name.GetValue()));
        }

        public void Complete()
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule("Status", "Project is already completed");

            if (GetPropsCopy().Status == ProjectStatus.OnHold)
                AddBrokenRule("Status", "Project is on hold, you can't complete it");

            var props = GetProps();

            props.Status = ProjectStatus.Completed;
            props.Audit.Update("default");

            SetProps(props);

            AddDomainEvent(new ProjectCompletedDomainEvent(GetPropsCopy().Id.GetValue(), GetPropsCopy().Name.GetValue()));
        }

        public void Hold()
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule("Status", "Project is completed, you can't hold it");

            if (GetPropsCopy().Status == ProjectStatus.OnHold)
                AddBrokenRule("Status", "Project is already on hold");

            var props = GetProps();

            props.Status = ProjectStatus.OnHold;
            props.Audit.Update("default");

            SetProps(props);

            AddDomainEvent(new ProjectHoldedDomainEvent(GetPropsCopy().Id.GetValue(), GetPropsCopy().Name.GetValue()));
        }

        public void AddBacklog(ProjectBacklog backlog)
        {
            if (GetPropsCopy().Backlogs!.Any(x => x.GetPropsCopy().Name.ToString()!.Equals(backlog.GetPropsCopy().Name.ToString(), StringComparison.OrdinalIgnoreCase)))
                return;

            var props = GetProps();

            props.Backlogs!.Add(backlog);
            props.Audit.Update("default");

            SetProps(props);
        }

        public void RemoveBacklog(ProjectBacklog backlog)
        {
            if (GetPropsCopy().Backlogs!.Any(x => x.GetPropsCopy().Name.ToString()!.Equals(backlog.GetPropsCopy().Name.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                var props = GetProps();

                props.Backlogs!.Remove(backlog);
                props.Audit.Update("default");

                SetProps(props);
            }
        }

        public void AddScope(ProjectScope scope)
        {
            if (GetPropsCopy().Scopes!.Any(x => x.GetValue().Description.ToString()!.Equals(scope.GetValue().Description.ToString(), StringComparison.OrdinalIgnoreCase)))
                return;

            var props = GetProps();

            props.Scopes!.Add(scope);
            props.Audit.Update("default");

            SetProps(props);
        }

        public void RemoveScope(ProjectScope scope)
        {
            if (GetPropsCopy().Scopes!.Any(x => x.GetValue().Description.ToString().ToLower()!.Equals(scope.GetValue().Description.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                var props = GetProps();
                
                props.Scopes!.Remove(scope);
                props.Audit.Update("default");

                SetProps(props);
            }
        }

        public void AddBudget(Price budget)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(budget), "Project is completed, you can't add budget");

            var props = GetProps();
            
            props.Budget = budget;
            props.Audit.Update("default");

            SetProps(props);
        }

        public void AddExtraBudget(Price extraBudget)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(extraBudget), "Project is completed, you can't add extra budget");

            var props = GetProps();

            props.ExtraBudget!.Add(extraBudget);
            props.Audit.Update("default");

            SetProps(props);
        }

        public void AddStakeholder(Stakeholder stakeholder)
        {
            if (GetPropsCopy().StakeHolders!.Any(x => x.GetValue().Name.ToString()!.Equals(stakeholder.GetValue().Name.ToString(), StringComparison.OrdinalIgnoreCase)))
                return;

            var props = GetProps();

            props.StakeHolders!.Add(stakeholder);
            props.Audit.Update("default");

            SetProps(props);
        }

        public void RemoveStakeholder(Stakeholder stakeholder)
        {
            if (GetPropsCopy().StakeHolders!.Any(x => x.GetValue().Name.ToString().ToLower()!.Equals(stakeholder.GetValue().Name.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                var props = GetProps();
                
                props.StakeHolders!.Remove(stakeholder);
                props.Audit.Update("default");

                SetProps(props);
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
