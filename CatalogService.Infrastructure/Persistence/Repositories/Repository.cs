using CatalogService.Domain.Abstractions;
using CatalogService.Domain.IRepositories;
using System.Linq.Expressions;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T>
    where T : Entity
{
    protected ApplicationDbContext _context;
    private readonly ILogger<Repository<T>> _logger;
    private readonly DbSet<T> _dbSet;

    public Repository(
        ApplicationDbContext context,
        ILogger<Repository<T>> logger
        )
    {
        _context = context;
        _logger = logger;
        _dbSet = _context.Set<T>();
    }
    public async Task<Guid> AddAsync(T entity, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        try
        {
            await _dbSet.AddAsync(entity, ct);
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Added entity of type {EntityType} with ID {EntityId}", typeof(T).Name, entity.Id);
            }
            return entity.Id;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error adding entity of type {EntityType}", typeof(T).Name);
            throw new InvalidOperationException($"Error adding entity of type {typeof(T).Name}", ex);
        }
    }
    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(entities);
        if (!entities.Any())
            return;
        try
        {
            await _dbSet.AddRangeAsync(entities, ct);
            await _context.SaveChangesAsync(true, ct);
            _logger.LogInformation("Added {Count} entities of type {EntityType}", entities.Count(), typeof(T).Name);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error adding multiple entities of type {EntityType}", typeof(T).Name);
            throw new InvalidOperationException($"Error adding multiple entities of type {typeof(T).Name}", ex);
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _dbSet.FindAsync([id], ct);
        if (entity is null)
        {
            _logger.LogWarning("Entity of type {EntityType} with ID {EntityId} not found for deletion", typeof(T).Name, id);
            throw new InvalidOperationException($"Entity of type {typeof(T).Name} with ID {id} not found");
        }
        try
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(true, ct);
            _logger.LogInformation("Deleted entity of type {EntityType} with ID {EntityId}", typeof(T).Name, id);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting entity of type {EntityType} with ID {EntityId}", typeof(T).Name, id);
            throw new InvalidOperationException($"Error deleting entity of type {typeof(T).Name} with ID {id}", ex);
        }
    }
    public async Task<int> DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        try
        {
            if (await _dbSet.SingleOrDefaultAsync(predicate, ct) is not { } lowStock)
                return 0;

            _logger.LogInformation("Deleted entity of type {EntityType} with ID {EntityId}", typeof(T).Name, lowStock.Id);
            _dbSet.Remove(lowStock);
            return await _context.SaveChangesAsync(true, ct);
        }
        catch
        {
            _logger.LogError("Error deleting entity of type {EntityType} matching predicate", typeof(T).Name);
            throw new InvalidOperationException($"Error deleting entity of type {typeof(T).Name} matching predicate");
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
    {
        var exists = await _dbSet.AnyAsync(e => e.Id == id, ct);
        _logger.LogDebug("Checked existence for entity of type {EntityType} with ID {EntityId}: {Exists}", typeof(T).Name, id, exists);
        return exists;
    }
    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        try
        {
            var exists = await _dbSet.AnyAsync(predicate, ct);
            _logger.LogDebug("Checked existence for entity of type {EntityType} matching predicate: {Exists}", typeof(T).Name, exists);
            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence for entity of type {EntityType} matching predicate", typeof(T).Name);
            throw new InvalidOperationException($"Error checking existence for entity of type {typeof(T).Name} matching predicate", ex);
        }
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var entity = await _dbSet.FindAsync([id], ct);
            if (entity != null)
                _logger.LogInformation("Retrieved entity of type {EntityType} with ID {EntityId}", typeof(T).Name, id);
            else
                _logger.LogWarning("Entity of type {EntityType} with ID {EntityId} not found", typeof(T).Name, id);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving entity of type {EntityType} with ID {EntityId}", typeof(T).Name, id);
            throw new InvalidOperationException($"Error retrieving entity of type {typeof(T).Name} with ID {id}", ex);
        }
    }

    public async Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (entity.Id == Guid.Empty)
            throw new ArgumentException($"Invalid ID for entity of type {typeof(T).Name}", nameof(entity));
        try
        {
            var existingEntity = await _dbSet.FindAsync([entity.Id], ct);
            if (existingEntity is null)
            {
                _logger.LogWarning("Entity of type {EntityType} with ID {EntityId} not found for update", typeof(T).Name, entity.Id);
                throw new InvalidOperationException($"Entity of type {typeof(T).Name} with ID {entity.Id} not found");
            }
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            _logger.LogInformation("Updated entity of type {EntityType} with ID {EntityId}", typeof(T).Name, entity.Id);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating entity of type {EntityType} with ID {EntityId}", typeof(T).Name, entity.Id);
            throw new InvalidOperationException($"Error updating entity of type {typeof(T).Name} with ID {entity.Id}", ex);
        }
    }
}
