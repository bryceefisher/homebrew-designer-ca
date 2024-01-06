using HomebrewDesigner.Core.DTO;

namespace HomebrewDesigner.Core.ServiceContracts;

public interface IFermentableService
{
    /// <summary>
    /// Adds a Fermentables object to the list of Grains.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Recently added grain object</returns>
    Task<FermentableResponse> AddFermentableAsync(FermentableAddRequest request);
    
    /// <summary>
    /// Returns all Grains from the list.
    /// </summary>
    /// <returns>A list of FermentableResponse objects.</returns>
   Task<List<FermentableResponse>> GetAllFermentablesAsync();
    
    /// <summary>
    /// Updates specified properties of a selected Fermentables object.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Recently Updated Fermentables Response Object.</returns>
    Task<FermentableResponse> UpdateFermentableAsync(FermentableUpdateRequest request);
    
    /// <summary>
    /// Returns a selected Fermentables object when supplied with an id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>FermentableResponse object with specified id.</returns>
    Task<FermentableResponse> GetFermentableByIdAsync(int id);
    
    /// <summary>
    /// Returns all Fermentable objects that match with the given search field and search string
    /// </summary>
    /// <param name="searchBy"></param>
    /// <param name="searchString"></param>
    /// <returns>List of Fermentable Objects</returns>
    Task<List<FermentableResponse>> GetFilteredFermentableAsync(string? searchBy, string? searchString);
}