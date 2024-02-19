using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.Helpers;
using HomebrewDesigner.Core.RepositoryContracts;
using HomebrewDesigner.Core.ServiceContracts;

namespace HomebrewDesignerTests.FakeRepos;

public class FakeRecipeRepo : IRecipeRepository
{
    // Create a list to hold hop objects.
    private readonly List<Recipe> _recipes = new();
    
    public async Task<Recipe> AddAsync(Recipe recipe)
    {
        _recipes.Add(recipe);
        return await Task.Run(() => recipe);
    }

    public async Task<List<Recipe>> GetAllAsync()
    {
        return await Task.Run(() => _recipes);
    }

    public Task<Recipe> UpdateAsync(Recipe recipe, RecipeUpdateRequest request)
    {
        Recipe recipeToUpdate = _recipes.Find(r => r.Id == recipe.Id) ?? throw new ArgumentException("Hop not found.");
        
        recipeToUpdate.Id = request.Id;
        recipeToUpdate.Name = request.Name;
        recipeToUpdate.Style = request.Style.ToString();
        recipeToUpdate.OriginalGravity = request.OriginalGravity;
        recipeToUpdate.FinalGravity = request.FinalGravity;
        recipeToUpdate.IBU = request.IBU;
        recipeToUpdate.ABV = request.ABV;
        recipeToUpdate.HopAdditions = request.Hops;
        recipeToUpdate.Color = request.Color;
        
        return Task.Run(() => recipeToUpdate);
    }

    public async Task<Recipe> GetByIdAsync(int id)
    {
        return await Task.Run(() => _recipes.Find(r => r.Id == id)) ?? throw new ArgumentException("Hop not found");
    }

    public async Task<IEnumerable<Recipe>> GetLastThreeEntriesAsync()
    {
        return _recipes.OrderByDescending(recipe => recipe.Id)
            .Take(3);
    }


    //To-do Figure out how to implement a fake version of this and test
    public IEnumerable<RecipeResponse> GetFilteredRecipes(string? searchBy, string? searchString)
    {
        throw new NotImplementedException();
    }
}