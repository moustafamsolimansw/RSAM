using RSAM.Domain.SharedModels;
using System.Linq.Expressions;

namespace RSAM.Application.Repositories;

public interface IReadRepository<T, TId> where T : Entity<TId> where TId : ValueObject
{
    IQueryable<T> AsQueryable(bool asNoTracking = true);

    Task<T?> GetByIdAsync(TId id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>>? predicate = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes);

    Task<List<T>> GetListAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool asNoTracking = true,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes);

    Task<bool> AnyAsync(
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    Task<TResult?> MaxAsync<TResult>(
        Expression<Func<T, TResult>> selector,
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TResult>> GetGroupedAsync<TKey, TResult>(
        Expression<Func<T, TKey>> groupingKey,
        Expression<Func<IGrouping<TKey, T>, TResult>> resultSelector,
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool asNoTracking = true,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes);

    Task<IEnumerable<TResult>> GetSpecificSelectAsync<TResult>(
        Expression<Func<T, TResult>> select,
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool asNoTracking = true,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes);

    Task<List<object>> DynamicLookupAsync(string lang, CancellationToken cancellationToken = default);
}
