using RSAM.Application.Repositories;
using RSAM.Domain.SharedModels;
using RSAM.Infrastructure.Context;
using System.Collections.Concurrent;

namespace RSAM.Infrastructure.Repositories;

public class ReadUnitOfWork : IReadUnitOfWork
{
    private readonly RSAMDbContext _context;
    private readonly ConcurrentDictionary<string, object> _repositories;

    public ReadUnitOfWork(RSAMDbContext context)
    {
        _context = context;
        _repositories = new ConcurrentDictionary<string, object>();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public IReadRepository<T, TId> ReadRepository<T, TId>()
        where T : Entity<TId>
        where TId : ValueObject
    {
        var type = typeof(T).Name;

        return (IReadRepository<T, TId>)_repositories.GetOrAdd(type, _ => new ReadRepository<T, TId>(_context));
    }
}
