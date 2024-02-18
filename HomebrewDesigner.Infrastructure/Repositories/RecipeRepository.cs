using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.RepositoryContracts;
using HomebrewDesigner.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace HomebrewDesigner.Infrastructure.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly ApplicationDbContext _db;

    public RecipeRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Recipe> AddAsync(Recipe recipe)
    {
        _db.Recipes.Add(recipe);
        await _db.SaveChangesAsync();
        
        return recipe;
    }

    public async Task<List<Recipe>> GetAllAsync()
    {
        return await _db.Recipes
            .Include(r => r.Yeast)
            .Include(r => r.HopAdditions)
            .ThenInclude(ha => ha.Hop)
            .Include(r => r.Maltbill)
            .ThenInclude(fp => fp.Fermentable)
            .ToListAsync();
    }

    public async Task<Recipe> GetByIdAsync(int id)
    {
        return await _db.Recipes
            .Include(r => r.Yeast)
            .Include(r => r.HopAdditions)
            .ThenInclude(ha => ha.Hop)
            .Include(r => r.Maltbill)
            .ThenInclude(fp => fp.Fermentable)
            .FirstOrDefaultAsync(r => r.Id == id) ?? throw new InvalidOperationException();
    }

    public async Task<Recipe> UpdateAsync(Recipe recipe, RecipeUpdateRequest request)
    {
        Recipe recipeToUpdate = await _db.Recipes.FirstOrDefaultAsync(r => r.Id == recipe.Id) ?? throw new InvalidOperationException();
        
        recipeToUpdate.Name = request.Name;
        recipeToUpdate.Style = request.Style.ToString();
        recipeToUpdate.YeastAmount = request.YeastAmount;
        recipeToUpdate.YeastViability = request.YeastViability;
        recipeToUpdate.MashTemp = request.MashTemp;
        recipeToUpdate.WaterRatio = request.WaterRatio;
        
        await _db.SaveChangesAsync();
        
        return recipeToUpdate;
    }

    public async Task<IEnumerable<Recipe>> GetLastThreeEntriesAsync()
    {
        var lastThreeEntries = await _db.Recipes
            .Include(r => r.Yeast)
            .Include(r => r.HopAdditions)
            .ThenInclude(ha => ha.Hop)
            .Include(r => r.Maltbill)
            .ThenInclude(fp => fp.Fermentable).ToListAsync();

       return lastThreeEntries.OrderByDescending(recipe => recipe.Id)
            .Take(3);
        
    }
}