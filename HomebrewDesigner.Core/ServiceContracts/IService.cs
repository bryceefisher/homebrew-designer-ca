using HomebrewDesigner.Core.DTO;

namespace HomebrewDesigner.Core.ServiceContracts;

public interface IService<in TAddRequest, in TUpdateRequest, TResponse>
{
    /// <summary>
    /// Adds an object to the list
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<TResponse> AddAsync(TAddRequest? request);
    
    /// <summary>
    /// Returns all objects from the list
    /// </summary>
    /// <returns></returns>
    Task<List<TResponse>> GetAllAsync();
    
    /// <summary>
    /// Updates specified properties of a selected object.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>TResponse Object</returns>
    Task<TResponse> UpdateAsync(TUpdateRequest? request);
    
    /// <summary>
    /// Returns an object when supplied with an id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TResponse> GetByIdAsync(int id);
    
    /// <summary>
    /// Returns all objects that match with the given search field and search string
    /// </summary>
    /// <param name="searchBy"></param>
    /// <param name="searchString"></param>
    /// <returns>List of TResponse Objects</returns>
    Task<List<TResponse>> GetFilteredAsync(string? searchBy, string? searchString);
    
    /// <summary>
    /// Deletes an object based on the given id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Bool indicating successful deletion</returns>
    Task<bool> DeleteAsync(int id);
}
