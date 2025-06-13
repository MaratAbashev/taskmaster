using System.Linq.Expressions;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public abstract class Repository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : struct
{
    protected readonly DbSet<TEntity> _dbSet;
    protected readonly AppDbContext context;

    protected Repository(AppDbContext context)
    {
        this.context = context;
        _dbSet = this.context.Set<TEntity>();
    }

    protected virtual async Task<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _dbSet.FirstOrDefaultAsync(filter);
    }

    protected virtual async Task<TEntity?> GetByFilterWithoutTrackingAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(filter);
    }
    
    protected virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }
    
    protected virtual async Task<IEnumerable<TEntity?>> GetAllByFilterWithoutTrackingAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(filter)
            .ToListAsync();
    }
    
    protected virtual async Task<IEnumerable<TEntity?>> GetAllByFilterAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _dbSet
            .Where(filter)
            .ToListAsync();
    }
    
    protected virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        await _dbSet
            .Where(predicate)
            .ExecuteDeleteAsync();
    }

    protected virtual async Task<TEntity> PatchAsync(TId id, Action<TEntity> patch)
    {
        var existingEntity = await _dbSet
            .FirstAsync(e => e.Id.Equals(id));
        if (existingEntity == null)
        {
            throw new KeyNotFoundException("Entity not found");
        }
        patch(existingEntity);
        await context.SaveChangesAsync();
        return existingEntity;
    }
}