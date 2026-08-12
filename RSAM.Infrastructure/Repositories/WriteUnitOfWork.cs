using RSAM.Application.Repositories;
using RSAM.Domain.SharedModels;
using RSAM.Infrastructure.Context;
using System.Collections.Concurrent;

namespace RSAM.Infrastructure.Repositories;

public class WriteUnitOfWork : IWriteUnitOfWork
{
    private readonly RSAMDbContext _context;
    private readonly ConcurrentDictionary<string, object> _repositories;

    public WriteUnitOfWork(RSAMDbContext context)
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

    public IWriteRepository<T, TId> WriteRepository<T, TId>()
        where T : BaseEntity<TId>
        where TId : ValueObject
    {
        var type = typeof(T).Name;

        return (IWriteRepository<T, TId>)_repositories.GetOrAdd(type, _ => new WriteRepository<T, TId>(_context));
    }
}
