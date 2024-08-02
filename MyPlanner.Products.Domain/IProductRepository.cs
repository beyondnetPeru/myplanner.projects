using BeyondNet.Ddd.Interfaces;


namespace MyPlanner.Products.Domain
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetAsync(string id);
        Task Add(Product product);
        Task ChangeName(string id, string name);
        Task ChangeDescription(string id, string description);
        Task Delete(string id);
    }
}
