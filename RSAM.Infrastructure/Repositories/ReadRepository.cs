using Microsoft.EntityFrameworkCore;
using RSAM.Application.Repositories;
using RSAM.Domain.SharedModels;
using RSAM.Infrastructure.Context;
using System.Linq.Expressions;

namespace RSAM.Infrastructure.Repositories;

public class ReadRepository<T, TId> : IReadRepository<T, TId>
    where T : Entity<TId>
    where TId : ValueObject
{
    private readonly RSAMDbContext _rsamdbContext;
    private readonly DbSet<T> _dbSet;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ReadRepository() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    
    public ReadRepository(RSAMDbContext rsamdbContext)
    {
        _rsamdbContext = rsamdbContext;
        _dbSet = rsamdbContext.Set<T>();
    }

    public IQueryable<T> AsQueryable(bool asNoTracking = true)
    {
        return asNoTracking ? _dbSet.AsNoTracking() : _dbSet;
    }

    public async Task<T?> GetByIdAsync(TId id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        return await AsQueryable(asNoTracking).FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
    }

    public async Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>>? predicate = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = AsQueryable(asNoTracking);

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (predicate != null)
            query = query.Where(predicate);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<T>> GetListAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool asNoTracking = true,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = AsQueryable(asNoTracking);

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (predicate != null)
            query = query.Where(predicate);

        if (orderBy != null)
            query = orderBy(query);

        if (skip.HasValue && skip.Value > 0)
            query = query.Skip(skip.Value);

        if (take.HasValue && take.Value > 0)
            query = query.Take(take.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        return predicate == null 
            ? await _dbSet.AsNoTracking().AnyAsync(cancellationToken) 
            : await _dbSet.AsNoTracking().AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        return predicate == null 
            ? await _dbSet.AsNoTracking().CountAsync(cancellationToken) 
            : await _dbSet.AsNoTracking().CountAsync(predicate, cancellationToken);
    }

    public async Task<TResult?> MaxAsync<TResult>(
        Expression<Func<T, TResult>> selector,
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking();
        
        if (predicate != null)
            query = query.Where(predicate);

        if (await query.AnyAsync(cancellationToken))
            return await query.MaxAsync(selector, cancellationToken);
            
        return default;
    }

    public async Task<IEnumerable<TResult>> GetGroupedAsync<TKey, TResult>(
        Expression<Func<T, TKey>> groupingKey,
        Expression<Func<IGrouping<TKey, T>, TResult>> resultSelector,
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool asNoTracking = true,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = AsQueryable(asNoTracking);

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (predicate != null)
            query = query.Where(predicate);

        if (orderBy != null)
            query = orderBy(query);
            
        if (skip.HasValue && skip.Value > 0)
            query = query.Skip(skip.Value);

        if (take.HasValue && take.Value > 0)
            query = query.Take(take.Value);

        return await query.GroupBy(groupingKey).Select(resultSelector).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TResult>> GetSpecificSelectAsync<TResult>(
        Expression<Func<T, TResult>> select,
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool asNoTracking = true,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = AsQueryable(asNoTracking);

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (predicate != null)
            query = query.Where(predicate);

        if (orderBy != null)
            query = orderBy(query);

        if (skip.HasValue && skip.Value > 0)
            query = query.Skip(skip.Value);

        if (take.HasValue && take.Value > 0)
            query = query.Take(take.Value);

        return await query.Select(select).ToListAsync(cancellationToken);
    }

    public async Task<List<object>> DynamicLookupAsync(string lang, CancellationToken cancellationToken = default)
    {
        // A truly dynamic lookup often requires projecting to a specific DropdownDto based on the 'lang' parameter.
        // For a generic repository, we return the entities as objects, allowing the caller to map them appropriately.
        return await _dbSet.AsNoTracking().Cast<object>().ToListAsync(cancellationToken);
    }
}
