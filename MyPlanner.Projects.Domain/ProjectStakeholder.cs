﻿using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;


namespace MyPlanner.Projects.Domain
{
    public class ProjectStakeHolderProsp : IProps
    {
        public IdValueObject Id { get; set; }
        public Name Name { get; set; }
        public Project Project { get; set; }
        public IdValueObject ProjectId { get; set; }

        public ProjectStakeHolderProsp(IdValueObject id, Project project, Name name)
        {
            Id = id;
            Project = project;
            ProjectId = project.GetPropsCopy().Id;
            Name = name;
        }

        public object Clone()
        {
            return new ProjectStakeHolderProsp(Id, Project, Name);
        }
    }

    public class ProjectStakeholder : Entity<ProjectStakeholder, ProjectStakeHolderProsp>
    {
        private ProjectStakeholder(ProjectStakeHolderProsp props) : base(props)
        {
        }

        public static ProjectStakeholder Create(IdValueObject id, Project project, Name name)
        {
            return new ProjectStakeholder(new ProjectStakeHolderProsp(id, project, name));
        }

        public void UpdateName(Name name)
        {
            GetProps().Name.SetValue(name.GetValue());
        }
    }
}
