namespace BeyondNet.Ddd.Interfaces
{
    public interface IRepository<T> where T: class
    {    
        IUnitOfWork UnitOfWork { get; }
    }
}
