using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;

namespace HomebrewDesigner.Core.ServiceContracts;

public interface IRecipeService
{
    /// <summary>
    /// Adds a Recipe object to the list of Recipes
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<RecipeResponse> AddRecipeAsync(RecipeAddRequest? request);
    
    /// <summary>
    /// Returns all Recipes from the list
    /// </summary>
    /// <returns></returns>
    Task<List<RecipeResponse>> GetAllRecipesAsync();
    
    /// <summary>
    /// Updates specified properties of a selected Recipe object.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>HopResponse Object</returns>
    Task<RecipeResponse> UpdateRecipeAsync (RecipeUpdateRequest? request);
    
    /// <summary>
    /// Returns a Recipe object when supplied with an id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<RecipeResponse> GetRecipeByIdAsync(int id);

    /// <summary>
    /// Gets the last three entries in the Recipe list.
    /// </summary>
    /// <returns>Three most recent Recipes</returns>
    Task<IEnumerable<RecipeResponse>> GetLastThreeEntriesAsync();
    
    /// <summary>
    /// Gets the last three entries in the Recipe list.
    /// </summary>
    /// <returns>Three most recent Recipes</returns>
    Task<IEnumerable<RecipeResponse>> GetFilteredRecipesAsync(string? searchBy, string? searchString);
}