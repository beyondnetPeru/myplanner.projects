using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;
using MyPlanner.Products.Domain;
using MyPlanner.Products.Infrastructure.Database;
using MyPlanner.Products.Infrastructure.Database.Tables;
using MyPlanner.Shared.Domain.ValueObjects;

namespace MyPlanner.Products.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext context;

        public ProductRepository(ProductDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => context;

        public async Task Add(Product product)
        {
            await context.AddAsync(TransFromEntityToTable(product));
        }

        private static ProductTable TransFromEntityToTable(Product product)
        {
            return new ProductTable
            {
                Id = product.GetPropsCopy().Id.GetValue(),
                Name = product.GetPropsCopy().Name.GetValue()
            };
        }

        public async Task ChangeDescription(string id, string description)
        {
            var productTable = await FindAsync(id);

            productTable.Description = description;
        }

        public async Task ChangeName(string id, string name)
        {
            var productTable = await FindAsync(id);

            productTable.Name = name;
        }

        public async Task<ProductTable> FindAsync(string id)
        {
            var productTable = await context.Products.FindAsync(id);

            if (productTable == null)
            {
                throw new KeyNotFoundException($"Entity \"{nameof(ProductTable)}\" ({id}) was not found.");
            }

            return productTable;
        }

        public async Task<Product> GetAsync(string id)
        {
            var productTable = await FindAsync(id);

            return Product.Create(IdValueObject.Create(productTable.Id),
                        Name.Create(productTable.Name));
        }

        public async Task Delete(string id)
        {
            var productTable = await FindAsync(id);

            context.Products.Remove(productTable);
        }
    }
}
