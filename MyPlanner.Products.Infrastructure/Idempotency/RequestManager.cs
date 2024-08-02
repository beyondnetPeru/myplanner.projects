
using MyPlanner.Products.Domain;
using MyPlanner.Products.Infrastructure.Database;
using MyPlanner.Shared.Infrastructure.Idempotency;

namespace MyPlanner.Products.Infrastructure.Idempotency;

public class RequestManager : IRequestManager
{
    private readonly ProductDbContext _context;

    public RequestManager(ProductDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }


    public async Task<bool> ExistAsync(Guid id)
    {
        var request = await _context.
            FindAsync<ClientRequest>(id);

        return request != null;
    }

    public async Task CreateRequestForCommandAsync<T>(Guid id)
    {
        var exists = await ExistAsync(id);

        var request = exists ?
            throw new ProductDomainException($"Request with {id} already exists") :
            new ClientRequest()
            {
                Id = id,
                Name = typeof(T).Name,
                Time = DateTime.UtcNow
            };

        _context.Add(request);

        await _context.SaveChangesAsync();
    }
}
