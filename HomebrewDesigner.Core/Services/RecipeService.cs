using System.Reflection;
using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.Helpers;
using HomebrewDesigner.Core.RepositoryContracts;
using HomebrewDesigner.Core.ServiceContracts;
using ArgumentException = System.ArgumentException;

namespace HomebrewDesigner.Core.Services;

public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IHopRepository _hopRepository;
    private readonly IFermentableRepository _fermentableRepository;
    private readonly IYeastRepository _yeastRepository;

    public RecipeService(IRecipeRepository recipeRepository, IHopRepository hopRepository,
        IFermentableRepository fermentableRepository, IYeastRepository yeastRepository)
    {
        _recipeRepository = recipeRepository;
        _hopRepository = hopRepository;
        _fermentableRepository = fermentableRepository;
        _yeastRepository = yeastRepository;
    }

    public async Task<RecipeResponse> AddAsync(RecipeAddRequest? request)
    {
        if (request is null)
        {
            throw new ArgumentException("Recipe object cannot be null.");
        }

        Recipe recipeToAdd = request.ToRecipe();

        ValidationHelper.ModelValidation(request);


        foreach (var hopAddition in recipeToAdd.HopAdditions)
        {
            var existingHop = await _hopRepository.GetHopByIdAsync(hopAddition.Hop.Id);
            if (existingHop != null)
            {
                hopAddition.Hop = existingHop; // Associate the existing hop
            }
        }


        foreach (var maltBill in recipeToAdd.Maltbill)
        {
            var existingFermentable = await _fermentableRepository.GetFermentableByIdAsync(maltBill.Fermentable.Id);
            if (existingFermentable != null)
            {
                maltBill.Fermentable = existingFermentable; // Associate the existing fermentable
            }
        }

        var existingYeast = await _yeastRepository.GetYeastByIdAsync(recipeToAdd.Yeast.Id);
        if (existingYeast != null)
        {
            recipeToAdd.Yeast = existingYeast; // Associate the existing yeast
        }

        await _recipeRepository.AddRecipeAsync(recipeToAdd);

        return recipeToAdd.ToRecipeResponse();
    }

    public async Task<List<RecipeResponse>> GetAllAsync()
    {
       List<Recipe> recipes =  await _recipeRepository.GetAllRecipesAsync();
       
       return recipes.Select(r => r.ToRecipeResponse()).ToList();
    }

    public async Task<RecipeResponse> UpdateAsync(RecipeUpdateRequest? request)
    {
        if (request is null)
        {
            throw new ArgumentException("Recipe object cannot be null.");
        }


        Recipe recipeToUpdate = await _recipeRepository.GetRecipeByIdAsync(request.Id);

        ValidationHelper.ModelValidation(request);

       Recipe recipe = await _recipeRepository.UpdateRecipeAsync(recipeToUpdate, request);
       
       return recipe.ToRecipeResponse();
    }

    public async Task<RecipeResponse> GetByIdAsync(int id)
    {
        if (id < 0)
        {
            throw new ArgumentException("Id cannot be less than 0.");
        }

        Recipe recipe = await _recipeRepository.GetRecipeByIdAsync(id);

        return recipe.ToRecipeResponse();
    }

    public async Task<IEnumerable<RecipeResponse>> GetLastThreeEntriesAsync()
    {
        IEnumerable<Recipe> recipes =  await _recipeRepository.GetLastThreeEntriesAsync();

        return recipes.Select(r => r.ToRecipeResponse());
    }


    /*******ToDo Tighten up this code************/
    public async Task<List<RecipeResponse>> GetFilteredAsync(string? searchBy, string? searchString)
    {
        PropertyInfo? property = null;

        List<PropertyInfo> propertyInfo = typeof(RecipeResponse).GetProperties().ToList();

        foreach (PropertyInfo prop in propertyInfo)
        {
            if (String.Equals(prop.ToString()?.Substring(prop.ToString().IndexOf(" ", StringComparison.Ordinal) + 1),
                    searchBy, StringComparison.OrdinalIgnoreCase))
            {
                property = prop;
                break;
            }
        }

        List<Recipe> recipes = await _recipeRepository.GetAllRecipesAsync();
        
        if (String.IsNullOrEmpty(searchString))
        {
            return recipes.Select(r => r.ToRecipeResponse()).ToList();
        }

        IEnumerable<RecipeResponse> recipe = recipes.Select(r => r.ToRecipeResponse());

        List<RecipeResponse> filteredRecipes = recipe
            .Where(r => property.GetValue(r).ToString().ToLower()
                .Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
        if (filteredRecipes.Any())
        {
            return filteredRecipes;
        }

        return recipes.Select(r => r.ToRecipeResponse()).ToList();
    }
}