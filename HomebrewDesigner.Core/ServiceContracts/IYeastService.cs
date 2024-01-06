using HomebrewDesigner.Core.DTO;

namespace HomebrewDesigner.Core.ServiceContracts;

public interface IYeastService
{
    /// <summary>
    /// Adds a Yeast object to the list of Yeasts
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<YeastResponse> AddYeastAsync(YeastAddRequest? request);
    
    /// <summary>
    /// Returns all Yeasts from the list
    /// </summary>
    /// <returns></returns>
    Task<List<YeastResponse>> GetAllYeastsAsync();
    
    /// <summary>
    /// Updates specified properties of a selected Yeast object.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<YeastResponse> UpdateYeastAsync(YeastUpdateRequest? request);
    
    /// <summary>
    /// Returns a Yeast object when supplied with an id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<YeastResponse> GetYeastByIdAsync(int id);
    
    /// <summary>
    /// Returns all yeast objects that match with the given search field and search string
    /// </summary>
    /// <param name="searchBy"></param>
    /// <param name="searchString"></param>
    /// <returns>List of YeastResponse Objects</returns>
    Task<List<YeastResponse>> GetFilteredYeastAsync(string? searchBy, string? searchString);
    
}