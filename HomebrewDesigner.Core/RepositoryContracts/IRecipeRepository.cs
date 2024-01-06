using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;

namespace HomebrewDesigner.Core.RepositoryContracts;

public interface IRecipeRepository
{
    /// <summary>
    /// Adds a new Recipe to the Recipe table.
    /// </summary>
    /// <param name="recipe"></param>
    /// <returns></returns>
    Task<Recipe> AddRecipeAsync(Recipe recipe);
    
    /// <summary>
    /// Returns all Recipes from the Recipe table as a list.
    /// </summary>
    /// <returns></returns>
    Task<List<Recipe>> GetAllRecipesAsync();
    
    /// <summary>
    /// Returns a Recipe by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Recipe> GetRecipeByIdAsync(int id);
    
    /// <summary>
    /// Updates a Recipe object in the Recipe table.
    /// </summary>
    /// <param name="recipe"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<Recipe> UpdateRecipeAsync(Recipe recipe, RecipeUpdateRequest request);

    /// <summary>
    /// Returns a list of the last three entries in the Recipe table.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Recipe>> GetLastThreeEntriesAsync();
}