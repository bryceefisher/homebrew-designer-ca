using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;

namespace HomebrewDesigner.Core.RepositoryContracts;

public interface IHopRepository
{
    /// <summary>
    /// Adds a new Hop to the Hop table.
    /// </summary>
    /// <param name="hop"></param>
    /// <returns></returns>
    Task<Hop> AddHopAsync(Hop hop);
    
    /// <summary>
    /// Returns all Hops from the Hop table as a list.
    /// </summary>
    /// <returns>All Hops from the Hop table as a list.</returns>
    Task<List<Hop>> GetAllHopsAsync();
    
    /// <summary>
    /// Updates specified properties of a selected Hop object.
    /// </summary>
    /// <param name="hop"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<Hop> UpdateHopAsync(Hop hop, HopUpdateRequest request);
    
    /// <summary>
    /// Gets a Hop object by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Hop> GetHopByIdAsync(int id);
}