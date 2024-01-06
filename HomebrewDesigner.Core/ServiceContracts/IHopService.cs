using HomebrewDesigner.Core.DTO;

namespace HomebrewDesigner.Core.ServiceContracts;

public interface IHopService
{
    /// <summary>
    /// Adds a Hop object to the list of Hops
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<HopResponse> AddHopAsync(HopAddRequest? request);
    
    /// <summary>
    /// Returns all Hops from the list
    /// </summary>
    /// <returns></returns>
    Task<List<HopResponse>> GetAllHopsAsync();
    
    /// <summary>
    /// Updates specified properties of a selected Hop object.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>HopResponse Object</returns>
    Task<HopResponse> UpdateHopAsync (HopUpdateRequest? request);
    
    /// <summary>
    /// Returns a hop object when supplied with an id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<HopResponse> GetHopByIdAsync(int id);
    
    
    /// <summary>
    /// Returns all hop objects that match with the given search field and search string
    /// </summary>
    /// <param name="searchBy"></param>
    /// <param name="searchString"></param>
    /// <returns>List of HopResponse Objects</returns>
    Task<List<HopResponse>> GetFilteredHopsAsync(string? searchBy, string? searchString);
}