using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;
using MyPlanner.Projects.Domain.DomainEvents;

namespace MyPlanner.Projects.Domain
{
    #region Props
    public class ProjectProps : IProps
    {
        public IdValueObject Id { get; private set; }
        public Name Name { get; private set; }
        public Description? Description { get; private set; }
        public StringValueObject? Track { get; private set; } 
        public Product? Product { get; private set; }
        public Owner? Owner { get; private set; }
        public ProjectRiskLevel RiskLevel { get; set; } = ProjectRiskLevel.Low;
        public ICollection<ProjectBudget> Budgets { get; private set; } = new List<ProjectBudget>();
        public ICollection<ProjectBacklog> Backlogs { get; private set; } = new List<ProjectBacklog>();
        public ICollection<ProjectScope> Scopes { get; private set; } = new List<ProjectScope>();
        public ICollection<ProjectStakeholder> StakeHolders { get; private set; } = new List<ProjectStakeholder>();
        public ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;
        public Audit Audit { get; private set; } = Audit.Create("default");

        public ProjectProps(IdValueObject id, Name name)
        {
            Id = id;
            Name = name;
            Description = Description.DefaultValue;
            Track = StringValueObject.DefaultValue;
            Product = Product.DefaultValue;
            Owner = Owner.DefaultValue;
            Status = ProjectStatus.NotStarted;
            Audit = Audit.Create("default");
        }

        public ProjectProps(IdValueObject id, Name name, StringValueObject track, Product product, Description description, 
                            ProjectRiskLevel riskLevel, ICollection<ProjectBacklog> backlogs, ICollection<ProjectScope> scopes, 
                            Owner owner, ICollection<ProjectStakeholder> stakeHolders)
        {
            Id = id;
            Name = name;
            Track = track;
            Product = product;
            Description = description;
            RiskLevel = riskLevel;
            Backlogs = backlogs;
            Scopes = scopes;
            Owner = owner;
            StakeHolders = stakeHolders;            
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
                Scopes = Scopes,
                Owner = Owner,
                StakeHolders = StakeHolders,
                Status = Status,
                Audit = Audit
            };
        }
    }
    
    #endregion
    
    public class Project : Entity<Project, ProjectProps>, IAggregateRoot
    {
        #region Constructors

        private Project(ProjectProps props) : base(props)
        {
            if (IsNew)
                AddDomainEvent(new ProjectCreatedDomainEvent(GetPropsCopy().Id.GetValue(), GetPropsCopy().Name.GetValue()));
        }
                #endregion

        #region Factory Methods

        public static Project Create(IdValueObject id, Name name)
        {
           return new Project(new ProjectProps(id, name));
        }

        public static Project Create(IdValueObject id, Name name, StringValueObject track, Product product, Description description,
                                                ProjectRiskLevel riskLevel, ICollection<ProjectBacklog> backlogs, ICollection<ProjectScope> scopes,
                                                                                    Owner owner, ICollection<ProjectStakeholder> stakeHolders)
        {
            return new Project(new ProjectProps(id, name, track, product, description, riskLevel, backlogs, scopes, owner, stakeHolders));
        }

        #endregion

        #region Methods

        public void UpdateTrack(StringValueObject track)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(track), "Project is completed, you can't update the track");

            var props = GetProps();

            props.Track!.SetValue(track.GetValue());
            props.Audit.Update("default");

            SetProps(props);
        }

        public void UpdateName(Name name)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(name), "Project is completed, you can't update the name");

            var props = GetProps();
            
            props.Name.SetValue(name.GetValue());
            props.Audit.Update("default");
            
            SetProps(props);
        }

        public void UpdateDescription(Description description)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(description), "Project is completed, you can't update the description");

            var props = GetProps();
            
            props.Description!.SetValue(description.GetValue());
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

        public void SetOwner(Owner owner)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(owner), "Project is completed, you can't update the owner");

            var props = GetProps();

            props.Owner!.SetValue(owner.GetValue());
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
            if (GetPropsCopy().Scopes!.Any(x => x.GetPropsCopy().Description.ToString()!.Equals(scope.GetPropsCopy().Description.ToString(), StringComparison.OrdinalIgnoreCase)))
                return;

            var props = GetProps();

            props.Scopes!.Add(scope);
            props.Audit.Update("default");

            SetProps(props);
        }

        public void RemoveScope(ProjectScope scope)
        {
            if (GetPropsCopy().Scopes!.Any(x => x.GetPropsCopy().Description.ToString().ToLower()!.Equals(scope.GetPropsCopy().Description.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                var props = GetProps();
                
                props.Scopes!.Remove(scope);
                props.Audit.Update("default");

                SetProps(props);
            }
        }

        public void AddStakeholder(ProjectStakeholder stakeholder)
        {
            if (GetPropsCopy().StakeHolders!.Any(x => x.GetPropsCopy().Name.ToString()!.Equals(stakeholder.GetPropsCopy().Name.ToString(), StringComparison.OrdinalIgnoreCase)))
                return;

            var props = GetProps();

            props.StakeHolders!.Add(stakeholder);
            props.Audit.Update("default");

            SetProps(props);
        }

        public void RemoveStakeholder(ProjectStakeholder stakeholder)
        {
            if (GetPropsCopy().StakeHolders!.Any(x => x.GetPropsCopy().Name.ToString().ToLower()!.Equals(stakeholder.GetPropsCopy().Name.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                var props = GetProps();
                
                props.StakeHolders!.Remove(stakeholder);
                props.Audit.Update("default");

                SetProps(props);
            }

        }

        public void AddBudget(ProjectBudget projectBudget)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(projectBudget), "Project is completed, you can't add budget");

            var props = GetProps();

            props.Budgets!.Add(projectBudget);
            
            props.Audit.Update("default");

            SetProps(props);
        }

        public void RemoveBudget(ProjectBudget projectBudget)
        {
            if (GetPropsCopy().Status == ProjectStatus.Completed)
                AddBrokenRule(nameof(projectBudget), "Project is completed, you can't remove budget");

            var props = GetProps();

            props.Budgets!.Remove(projectBudget);
            props.Audit.Update("default");

            SetProps(props);
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
