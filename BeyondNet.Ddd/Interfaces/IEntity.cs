﻿namespace BeyondNet.Ddd.Interfaces
{
    public interface IEntity<TEntity, TProps> where TEntity : Entity<TEntity, TProps>
            where TProps : class, IProps, IDomainEvents
    {
    }
}
