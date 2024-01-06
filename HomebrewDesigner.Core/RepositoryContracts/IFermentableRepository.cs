using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;

namespace HomebrewDesigner.Core.RepositoryContracts;

/// <summary>
/// Acts as the data access layer for the Fermentable table.
/// </summary>
public interface IFermentableRepository
{
    /// <summary>
    /// Adds a new Fermentable to the Fermentable table.
    /// </summary>
    /// <param name="fermentable"></param>
    /// <returns>Fermentable object</returns>
    Task<Fermentables> AddFermentableAsync(Fermentables fermentable);
    
    /// <summary>
    /// Returns all Fermentables from the Fermentable table as a list.
    /// </summary>
    /// <returns>All Fermentables from the Fermentable table as a list</returns>
    Task<List<Fermentables>> GetAllFermentablesAsync();
    
    /// <summary>
    /// Updates specified properties of a selected Fermentables object.
    /// </summary>
    /// <param name="fermentable">Fermentable to Update</param>
    /// <param name="request">Request objects with updated values</param>
    /// <returns></returns>
    Task<Fermentables> UpdateFermentableAsync(Fermentables fermentable, FermentableUpdateRequest request);
    
    /// <summary>
    /// Gets a Fermentable object by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Fermentable Object</returns>
    Task<Fermentables> GetFermentableByIdAsync(int id);
}