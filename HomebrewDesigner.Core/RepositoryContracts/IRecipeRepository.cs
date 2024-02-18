using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;

namespace HomebrewDesigner.Core.RepositoryContracts;

public interface IRecipeRepository : IRepository<Recipe, RecipeUpdateRequest>
{
    /// <summary>
    /// Returns a list of the last three entries in the Recipe table.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Recipe>> GetLastThreeEntriesAsync();
}