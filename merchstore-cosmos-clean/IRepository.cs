using MerchStore.Domain.Common;

namespace MerchStore.Domain.Interfaces;

/// <summary>
/// Defines the generic contract for a repository that handles basic CRUD operations.
/// </summary>
/// <typeparam name="TEntity">The type of entity the repository works with.</typeparam>
/// <typeparam name="TId">The type of the entity's primary key.</typeparam>
public interface IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : notnull
{
    /// <summary>
    /// Gets an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>The entity if found, or null otherwise.</returns>
    Task<TEntity?> GetByIdAsync(TId id);

    /// <summary>
    /// Gets all entities of this type.
    /// </summary>
    /// <returns>An enumerable of all entities.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Adds a new entity.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    Task AddAsync(TEntity entity);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Removes an entity.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    Task RemoveAsync(TEntity entity);
}
