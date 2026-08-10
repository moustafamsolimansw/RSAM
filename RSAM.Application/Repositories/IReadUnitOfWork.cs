using RSAM.Domain.SharedModels;

namespace RSAM.Application.Repositories;

public interface IReadUnitOfWork : IDisposable
{
    IReadRepository<T, TId> ReadRepository<T, TId>() where T : Entity<TId> where TId : ValueObject;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
