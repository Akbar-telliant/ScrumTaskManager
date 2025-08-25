using Microsoft.EntityFrameworkCore;
using ScrumMaster.Data;
using System.Linq.Expressions;

namespace ScrumMaster.Services;

/// <summary>
/// Generic CRUD service for EF Core entities with an 'Id' property.
/// </summary>
public class EntityDataService<T> where T : class
{
    private readonly ScrumMasterDbContext _context;
    private readonly DbSet<T> _dbSet;

    public EntityDataService(ScrumMasterDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    /// <summary>
    /// Adds a new entity and saves changes.
    /// </summary>
    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Gets all entities with optional navigation property includes.
    /// </summary>
    public async Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();

        foreach (var includeProperty in includeProperties)
            query = query.Include(includeProperty);

        return await query.ToListAsync();
    }

    /// <summary>
    /// Gets entities matching a condition, with optional navigation property includes.
    /// </summary>
    public async Task<List<T>> GetByConditionAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet.AsNoTracking().Where(predicate);

        foreach (var includeProperty in includeProperties)
            query = query.Include(includeProperty);

        return await query.ToListAsync();
    }

    /// <summary>
    /// Finds an entity by its Id.
    /// </summary>
    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    /// <summary>
    /// Updates an existing entity safely.
    /// </summary>
    public async Task UpdateAsync(T entity)
    {
        var keyProperty = typeof(T).GetProperty("Id");
        if (keyProperty == null) throw new InvalidOperationException($"Entity {typeof(T).Name} must have an 'Id' property.");

        var id = keyProperty.GetValue(entity);
        var existing = await _dbSet.FindAsync(id);
        if (existing == null) throw new KeyNotFoundException($"Entity with Id {id} not found.");

        _context.Entry(existing).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an entity by Id.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null) throw new KeyNotFoundException($"Entity with Id {id} not found.");

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
