using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;

namespace HomebrewDesigner.Core.RepositoryContracts;

public interface IYeastRepository
{
    /// <summary>
    /// Adds a new Yeast to the Yeast table.
    /// </summary>
    /// <param name="yeast"></param>
    /// <returns></returns>
    Task<Yeast> AddYeastAsync(Yeast yeast);
    
    /// <summary>
    /// Returns all Yeasts from the Yeast table as a list.
    /// </summary>
    /// <returns></returns>
    Task<List<Yeast>> GetAllYeastAsync();
     
    /// <summary>
    /// Returns a Yeast object by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Yeast> GetYeastByIdAsync(int id);
    
    /// <summary>
    /// Updates specified properties of a selected Yeast object.
    /// </summary>
    /// <param name="yeast"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<Yeast> UpdateYeastAsync(Yeast yeast, YeastUpdateRequest request);
    
}