using RSAM.Domain.SharedModels;

namespace RSAM.Application.Repositories;

public interface IWriteUnitOfWork : IDisposable
{
    IWriteRepository<T, TId> WriteRepository<T, TId>() where T : BaseEntity<TId> where TId : ValueObject;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
