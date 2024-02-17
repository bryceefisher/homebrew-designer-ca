using HomebrewDesigner.Core.DTO;

namespace HomebrewDesigner.Core.ServiceContracts;

public interface IRecipeService : IService<RecipeAddRequest, RecipeUpdateRequest, RecipeResponse>
{
    /// <summary>
    ///    Returns the last three recipe entries from the DB.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<RecipeResponse>> GetLastThreeEntriesAsync();
}