using RSAM.Domain.SharedModels;
using System.Linq.Expressions;

namespace RSAM.Application.Repositories;

public interface IWriteRepository<T, TId> where T : Entity<TId> where TId : ValueObject
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    
    void Remove(T entity, string deletedBy = "");
    void RemoveRange(IEnumerable<T> entities);
    
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    
    Task<bool> ExecuteDeleteAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
}
