namespace BeyondNet.Ddd.Interfaces
{
    public interface IEntity<TEntity, TProps> : IDomainEvents
            where TEntity : Entity<TEntity, TProps>
            where TProps : class, IProps
    {
    }
}
