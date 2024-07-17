namespace BeyondNet.Ddd.Interfaces
{
    public interface ICommandRepository<T> 
    {
        Task<bool> Create(T item);
        Task<bool> Update(T item);
        Task<bool> Delete(string id);
        IUnitOfWork UnitOfWork { get; }
    }
}
