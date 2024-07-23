using MyPlanner.Projects.Domain;
using MyPlanner.Projects.Infrastructure.Database;

namespace MyPlanner.Projects.Infrastructure.Idempotency;

public class RequestManager : IRequestManager
{
    private readonly ProjectDbContext _context;

    public RequestManager(ProjectDbContext context)
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
            throw new ProjectDomainException($"Request with {id} already exists") :
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
