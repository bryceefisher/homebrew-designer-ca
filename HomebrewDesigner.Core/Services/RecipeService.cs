using System.Reflection;
using System.Security.Cryptography;
using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.Helpers;
using HomebrewDesigner.Core.RepositoryContracts;
using HomebrewDesigner.Core.ServiceContracts;
using Microsoft.CodeAnalysis;
using ArgumentException = System.ArgumentException;

namespace HomebrewDesigner.Core.Services;

public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IRepository<Hop, HopUpdateRequest> _hopRepository;
    private readonly IRepository<Fermentables, FermentableUpdateRequest> _fermentableRepository;
    private readonly IRepository<Yeast, YeastUpdateRequest> _yeastRepository;
    private Random _random = new Random();

    public RecipeService(IRecipeRepository recipeRepository, IRepository<Hop, HopUpdateRequest> hopRepository,
        IRepository<Fermentables, FermentableUpdateRequest> fermentableRepository, IRepository<Yeast, YeastUpdateRequest> yeastRepository)
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
            var existingHop = await _hopRepository.GetByIdAsync(hopAddition.Hop.Id);
            if (existingHop != null)
            {
                hopAddition.Hop = existingHop; // Associate the existing hop
            }
        }


        foreach (var maltBill in recipeToAdd.Maltbill)
        {
            var existingFermentable = await _fermentableRepository.GetByIdAsync(maltBill.Fermentable.Id);
            if (existingFermentable != null)
            {
                maltBill.Fermentable = existingFermentable; // Associate the existing fermentable
            }
        }

        var existingYeast = await _yeastRepository.GetByIdAsync(recipeToAdd.Yeast.Id);
        if (existingYeast != null)
        {
            recipeToAdd.Yeast = existingYeast; // Associate the existing yeast
        }

        await _recipeRepository.AddAsync(recipeToAdd);

        return recipeToAdd.ToRecipeResponse();
    }

    public async Task<List<RecipeResponse>> GetAllAsync()
    {
       List<Recipe> recipes =  await _recipeRepository.GetAllAsync();
       
       return recipes.Select(r => r.ToRecipeResponse()).ToList();
    }

    public async Task<RecipeResponse> UpdateAsync(RecipeUpdateRequest? request)
    {
        if (request is null)
        {
            throw new ArgumentException("Recipe object cannot be null.");
        }


        Recipe recipeToUpdate = await _recipeRepository.GetByIdAsync(request.Id);

        ValidationHelper.ModelValidation(request);

       Recipe recipe = await _recipeRepository.UpdateAsync(recipeToUpdate, request);
       
       return recipe.ToRecipeResponse();
    }

    public async Task<RecipeResponse> GetByIdAsync(int id)
    {
        if (id < 0)
        {
            throw new ArgumentException("Id cannot be less than 0.");
        }

        Recipe recipe = await _recipeRepository.GetByIdAsync(id);

        return recipe.ToRecipeResponse();
    }

    public Task<RecipeResponse> GetByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<RecipeResponse>> GetLastThreeEntriesAsync()
    {
        IEnumerable<Recipe> recipes =  await _recipeRepository.GetLastThreeEntriesAsync();

        return recipes.Select(r => r.ToRecipeResponse());
    }

    public void UpdateRecipeDetails(RecipeDetailsDto recipe, RecipeDetailsDto recipeVm)
    {
        recipe.Recipe.Name = recipeVm.Recipe.Name;
        recipe.Recipe.Style = recipeVm.Recipe.Style;
        recipe.Recipe.YeastAmount = recipeVm.Recipe.YeastAmount;
        recipe.Recipe.YeastViability = recipeVm.Recipe.YeastViability;
        recipe.Recipe.MashTemp = recipeVm.Recipe.MashTemp;
        recipe.Recipe.WaterRatio = recipeVm.Recipe.WaterRatio;
    }

    public void CalculateWaterAmount(RecipeDetailsDto recipe, RecipeDetailsDto recipeVm)
    {
        double? grainWeight = 0;
        foreach (var grain in recipe.Recipe.MaltBill)
        {
            grainWeight += grain.Weight;
        }
    
        recipe.Recipe.AmountOfWater = (double)Math.Round((decimal)(grainWeight * recipeVm.Recipe.WaterRatio), 2);
    }

    public void CalculateOriginalGravity(RecipeDetailsDto recipe, RecipeDetailsDto recipeVm)
    {
        double? GU = 0;
        foreach (var grain in recipe.Recipe.MaltBill)
        {
            GU += double.Parse(grain.Fermentable.PotentialGravity.ToString()
                .Substring(grain.Fermentable.PotentialGravity.ToString().IndexOf(".") + 1, 3)) * grain.Weight;
        }
    
        recipe.Recipe.OriginalGravity = double.Parse(((GU / 6.75) / 1000 + 1).ToString().Substring(0, 5));
    }

    public void CalculateFinalGravity(RecipeDetailsDto recipe)
    {
        var originalGravity = recipe.Recipe.OriginalGravity;
        if (originalGravity < 1.040)
        {
            recipe.Recipe.FinalGravity = Math.Round(1.002 + _random.NextDouble() * (1.010 - 1.005), 3);
        }
        else if (originalGravity < 1.060)
        {
            recipe.Recipe.FinalGravity = Math.Round(1.002 + _random.NextDouble() * (1.015 - 1.010), 3);
        }
        else if (originalGravity < 1.080)
        {
            recipe.Recipe.FinalGravity = Math.Round(1.002 + _random.NextDouble() * (1.020 - 1.012), 3);
        }
        else
        {
            recipe.Recipe.FinalGravity = Math.Round(1.002 + _random.NextDouble() * (1.025 - 1.015), 3);
        }
    }

    public void CalculateABV(RecipeDetailsDto recipe)
    {
        recipe.Recipe.ABV =
            (double)Math.Round((decimal)((recipe.Recipe.OriginalGravity - recipe.Recipe.FinalGravity) * 131.25), 2);
    }

    public int CalculateIBU(RecipeDetailsDto recipe)
    {
        var abv = recipe.Recipe.ABV;
        if (abv < 5) return RandomNumberGenerator.GetInt32(12, 35);
        if (abv < 6) return RandomNumberGenerator.GetInt32(20, 45);
        if (abv < 7) return RandomNumberGenerator.GetInt32(30, 55);
        if (abv < 8) return RandomNumberGenerator.GetInt32(40, 65);
        if (abv < 9) return RandomNumberGenerator.GetInt32(40, 75);
        return RandomNumberGenerator.GetInt32(60, 90);
    }

    public void CalculateColor(RecipeDetailsDto recipe)
    {
        double? MCU = 0;
        foreach (var grain in recipe.Recipe.MaltBill)
        {
            MCU += grain.Fermentable.Color * grain.Weight;
        }
    
        recipe.Recipe.Color = (double)(MCU / 5);
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

        List<Recipe> recipes = await _recipeRepository.GetAllAsync();
        
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

    public async Task<bool> DeleteAsync(int id)
    {
        RecipeResponse? personToDelete = await GetByIdAsync(id);

        if (personToDelete is null)
        {
            return false;
        }

        await _recipeRepository.DeleteAsync(id);
        
        return true;
    }
}