using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
    public async Task<IActionResult> Home()
    {
        IEnumerable<RecipeResponse> recipes = await _recipeService.GetLastThreeEntriesAsync();
        
        return View(recipes);
    }
    

    
}