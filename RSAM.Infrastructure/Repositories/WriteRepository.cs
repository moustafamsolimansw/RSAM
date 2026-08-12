using Microsoft.EntityFrameworkCore;
using RSAM.Application.Repositories;
using RSAM.Domain.SharedModels;
using RSAM.Infrastructure.Context;
using System.Linq.Expressions;

namespace RSAM.Infrastructure.Repositories;

public class WriteRepository<T, TId> : IWriteRepository<T, TId> where T : BaseEntity<TId> where TId : ValueObject
{
    private readonly RSAMDbContext _rsamdbContext;
    private readonly DbSet<T> _dbSet;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public WriteRepository() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public WriteRepository(RSAMDbContext rsamdbContext)
    {
        _rsamdbContext = rsamdbContext;
        _dbSet = rsamdbContext.Set<T>();
    }
    
    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
        return entities;
    }

    public async Task<bool> ExecuteDeleteAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));
        return await _dbSet.Where(filter).ExecuteDeleteAsync(cancellationToken) > 0;
    }

    public void Remove(T entity, string deletedBy = "")
    {
        if (entity is null) return;

        var entry = _rsamdbContext.Entry(entity);

        if (entry.Metadata.FindProperty("IsDeleted") != null)
            entry.Property("IsDeleted").CurrentValue = true;
        else
        {
            _dbSet.Remove(entity);
            return;
        }

        if (entry.Metadata.FindProperty("DeletedAt") != null)
            entry.Property("DeletedAt").CurrentValue = DateTime.UtcNow;

        if (entry.Metadata.FindProperty("DeletedBy") != null)
            entry.Property("DeletedBy").CurrentValue = deletedBy;

        entry.State = EntityState.Modified;
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            Remove(entity);
        }
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void UpdateRange(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
    }
}
