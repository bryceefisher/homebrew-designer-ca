using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace HomebrewDesigner.Controllers;

[Route("[controller]/[action]")]
public class HomeController : Controller
{
    private readonly IRecipeService _recipeService;

    public HomeController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }
    

    [Route("/")]
    public async Task<IActionResult> Index()
    {
        IEnumerable<RecipeResponse> recipes = await _recipeService.GetLastThreeEntriesAsync();
        
        return View(recipes);
    }
    

    
}