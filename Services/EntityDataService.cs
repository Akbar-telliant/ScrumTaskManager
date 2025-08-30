using Microsoft.EntityFrameworkCore;
using ScrumMaster.Data;
using System.Linq.Expressions;

namespace ScrumMaster.Services;

/// <summary>
/// Generic CRUD service for EF Core entities with an 'Id' property.
/// </summary>
public class EntityDataService<T> where T : class
{
    /// <summary>
    /// EF Core DbContext instance.
    /// </summary>
    private readonly ScrumMasterDbContext m_Context;

    /// <summary>
    /// DbSet for the entity type.
    /// </summary>
    private readonly DbSet<T> m_DBSet;

    /// <summary>
    /// Initializes the service with DbContext.
    /// </summary>
    /// <param name="context">EF Core DbContext.</param>    
    public EntityDataService(ScrumMasterDbContext context)
    {
        m_Context = context;
        m_DBSet = m_Context.Set<T>();
    }

    /// <summary>
    /// Adds a new entity and saves changes. 
    /// </summary>
    /// <param name="entity">Entity to add.</param>
    /// <returns>The added entity.</returns></summary>
    public async Task<T> AddAsync(T entity)
    {
        await m_DBSet.AddAsync(entity);
        await m_Context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Gets all entities with optional navigation property includes.
    /// </summary>
    /// <param name="includeProperties">Navigation properties to include.</param> 
    /// <returns>List of entities.</returns>
    public async Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = m_DBSet.AsNoTracking();

        foreach (var includeProperty in includeProperties)
            query = query.Include(includeProperty);

        return await query.ToListAsync();
    }

    /// <summary>
    /// Gets entities matching a condition, with optional includes.
    /// </summary>
    /// <param name="predicate">Filter condition.</param> 
    /// <param name="includeProperties">Navigation properties to include.</param>
    /// <returns>List of matching entities.</returns></summary>
    public async Task<List<T>> GetByConditionAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = m_DBSet.AsNoTracking().Where(predicate);

        foreach (var includeProperty in includeProperties)
            query = query.Include(includeProperty);

        return await query.ToListAsync();
    }

    /// <summary>
    /// Finds an entity by its Id.
    /// </summary>
    /// <param name="id">Entity Id.</param> 
    /// <returns>The found entity or null.</returns>
    public async Task<T?> GetByIdAsync(int id) => await m_DBSet.FindAsync(id);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">Entity with updated values.</param> 
    /// <returns>Task representing the operation.</returns>
    public async Task UpdateAsync(T entity)
    {
        var keyProperty = typeof(T).GetProperty("Id");
        if (keyProperty == null) throw new InvalidOperationException($"Entity {typeof(T).Name} must have an 'Id' property.");

        var id = keyProperty.GetValue(entity);
        var existing = await m_DBSet.FindAsync(id);
        if (existing == null) throw new KeyNotFoundException($"Entity with Id {id} not found.");

        m_Context.Entry(existing).CurrentValues.SetValues(entity);
        await m_Context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an entity by Id. 
    /// </summary>
    /// <param name="id">Entity Id.</param> 
    /// <returns>Task representing the operation.</returns>
    public async Task DeleteAsync(int id)
    {
        var entity = await m_DBSet.FindAsync(id);
        if (entity == null) throw new KeyNotFoundException($"Entity with Id {id} not found.");

        m_DBSet.Remove(entity);
        await m_Context.SaveChangesAsync();
    }
}
