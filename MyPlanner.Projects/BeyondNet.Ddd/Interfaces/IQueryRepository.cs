namespace BeyondNet.Ddd.Interfaces
{
    public interface IQueryRepository<T>
    {
        Task<IEnumerable<T>> GetAll(Pagination pagination);
        Task<T> GetById(string id);
        Task<bool> Exists(string id);
    }
}
