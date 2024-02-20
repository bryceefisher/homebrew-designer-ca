using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;

namespace HomebrewDesigner.Core.RepositoryContracts;

public interface IRepository<T, TUpdateRequest>
{
    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>The added entity.</returns>
    Task<T> AddAsync(T entity);
    
    /// <summary>
    /// Returns all entities from the repository as a list.
    /// </summary>
    /// <returns>All entities as a list.</returns>
    Task<List<T>> GetAllAsync();
    
    /// <summary>
    /// Updates specified properties of a selected entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="updateRequest">The update request containing the updated properties.</param>
    /// <returns>The updated entity.</returns>
    Task<T> UpdateAsync(T entity, TUpdateRequest updateRequest);
    
    /// <summary>
    /// Gets an entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <returns>The entity.</returns>
    Task<T> GetByIdAsync(int id);
    
    /// <summary>
    /// Deletes an object based on the given id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Bool indicating successful deletion</returns>
    Task<bool> DeleteAsync(int id);
}