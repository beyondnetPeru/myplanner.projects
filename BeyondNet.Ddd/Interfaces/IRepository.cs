namespace BeyondNet.Ddd.Interfaces
{
    public interface IRepository<T> where T: IAggregateRoot
    {    
        IUnitOfWork UnitOfWork { get; }
    }
}
