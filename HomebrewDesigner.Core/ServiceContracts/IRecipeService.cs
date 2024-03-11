using HomebrewDesigner.Core.DTO;

namespace HomebrewDesigner.Core.ServiceContracts;

public interface IRecipeService : IService<RecipeAddRequest, RecipeUpdateRequest, RecipeResponse>
{
    /// <summary>
    ///    Returns the last three recipe entries from the DB.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<RecipeResponse>> GetLastThreeEntriesAsync();

    /// <summary>
    /// Adds the recipe details to the recipeaddrequest object.
    /// </summary>
    /// <param name="recipe"></param>
    /// <param name="recipeVm"></param>
    void UpdateRecipeDetails(RecipeDetailsDto recipe, RecipeDetailsDto recipeVm);

    /// <summary>
    /// Calculates the water amount for the recipe.
    /// </summary>
    /// <param name="recipe"></param>
    /// <param name="recipeVm"></param>
    void CalculateWaterAmount(RecipeDetailsDto recipe, RecipeDetailsDto recipeVm);

    void CalculateOriginalGravity(RecipeDetailsDto recipe, RecipeDetailsDto recipeVm);
    
    void CalculateFinalGravity(RecipeDetailsDto recipe);

    void CalculateABV(RecipeDetailsDto recipe);

    int CalculateIBU(RecipeDetailsDto recipe);

    void CalculateColor(RecipeDetailsDto recipe);
}