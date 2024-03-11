using System.Security.Cryptography;
using System.Text.Json;
using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.ServiceContracts;
using HomebrewDesigner.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomebrewDesigner.Controllers;

[Route("[controller]/[action]")]
public class RecipeController : Controller
{
    private readonly IService<HopAddRequest, HopUpdateRequest, HopResponse> _hopService;
    private readonly IService<FermentableAddRequest, FermentableUpdateRequest, FermentableResponse> _fermentableService;
    private readonly IService<YeastAddRequest, YeastUpdateRequest, YeastResponse> _yeastService;
    private readonly IRecipeService _recipeService;
    private RecipeDetailsDto _recipeVm;

    public RecipeController(IService<HopAddRequest, HopUpdateRequest, HopResponse> hopService,
        IService<FermentableAddRequest, FermentableUpdateRequest, FermentableResponse> fermentableService,
        IService<YeastAddRequest, YeastUpdateRequest, YeastResponse> yeastService, IRecipeService recipeService)
    {
        _hopService = hopService;
        _fermentableService = fermentableService;
        _yeastService = yeastService;
        _recipeService = recipeService;
        _recipeVm = new RecipeDetailsDto(_hopService, _fermentableService, _yeastService);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Recipes(string? searchBy, string? searchString)
    {
        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            { nameof(RecipeResponse.Name), "Name" },
            { nameof(RecipeResponse.Style), "Style" },
            { nameof(RecipeResponse.OriginalGravity), "Original Gravity" },
            { nameof(RecipeResponse.FinalGravity), "Final Gravity" },
            { nameof(RecipeResponse.IBU), "IBU" },
            { nameof(RecipeResponse.ABV), "ABV" },
            { nameof(RecipeResponse.Color), "Color" }
        };

        IEnumerable<RecipeResponse> recipe = await _recipeService.GetFilteredAsync(searchBy, searchString);

        recipe = recipe.OrderBy(r => r.Name);


        if (recipe.Any())
        {
            return View(recipe);
        }

        return View();
    }

    // GET
    public async Task<IActionResult> AddRecipe()
    {
        var recipeVm = JsonSerializer.Deserialize<RecipeDetailsDto>(TempData["Recipe"].ToString());


        recipeVm.HopList = await _hopService.GetAllAsync();
        recipeVm.YeastList = await _yeastService.GetAllAsync();
        recipeVm.FermentableList = await _fermentableService.GetAllAsync();

        TempData["Recipe"] = JsonSerializer.Serialize(recipeVm);
        return View(recipeVm);
    }

    public async Task<IActionResult> AddFermentables()
    {
        RecipeVM recipeVm = new(_hopService, _fermentableService, _yeastService)
        {
            Recipe = new RecipeAddRequest()
            {
                MaltBill = new List<RecipeAddRequest.FermentablePair>(),
                Hops = new List<HopAddition>(),
            },
        };

        recipeVm.HopList = await _hopService.GetAllAsync();
        recipeVm.YeastList = await _yeastService.GetAllAsync();
        recipeVm.FermentableList = await _fermentableService.GetAllAsync();

        return View(recipeVm);
    }

    [HttpPost]
    public async Task<IActionResult> AddFermentables([Bind("FermentableId, FermentableWeight")] RecipeVM recipeVm)
    {
        recipeVm.HopList = await _hopService.GetAllAsync();
        recipeVm.YeastList = await _yeastService.GetAllAsync();
        recipeVm.FermentableList = await _fermentableService.GetAllAsync();
        recipeVm.Recipe = new RecipeAddRequest();
        recipeVm.Recipe.MaltBill = new List<RecipeAddRequest.FermentablePair>();


        if (recipeVm.FermentableId is not null)
        {
            for (int i = 0; i < recipeVm.FermentableId.Length; i++)
            {
                if (i + 1 > recipeVm.FermentableWeight.Length && i < recipeVm.FermentableId.Length)
                {
                    break;
                }

                // Get Fermentable by Id from _fermentableService
                FermentableResponse fermentableResponse =
                    await _fermentableService.GetByIdAsync(recipeVm.FermentableId[i]);

                Fermentables fermentable = fermentableResponse.ToFermentables();


                // Create a new FermentablePair
                RecipeAddRequest.FermentablePair fermentablePair = new RecipeAddRequest.FermentablePair
                {
                    Fermentable = fermentable,
                    Weight = recipeVm.FermentableWeight[i]
                };

                // Add the FermentablePair to the MaltBill
                recipeVm.Recipe.MaltBill.Add(fermentablePair);
            }
        }

        TempData["Recipe"] = JsonSerializer.Serialize(recipeVm);

        return RedirectToAction("AddHops");
    }


    [HttpPost]
    public async Task<IActionResult> AddRecipe(RecipeDetailsDto recipeVm)
    {
        var recipe = JsonSerializer.Deserialize<RecipeDetailsDto>(TempData["Recipe"].ToString());

        if (recipeVm.YeastId != 0)
        {
            YeastResponse yeastResponse = await _yeastService.GetByIdAsync(recipeVm.YeastId);
            recipe.Recipe.Yeast = yeastResponse.ToYeast();
        }

        _recipeService.UpdateRecipeDetails(recipe, recipeVm);
        _recipeService.CalculateWaterAmount(recipe, recipeVm);
        _recipeService.CalculateOriginalGravity(recipe, recipeVm);
        _recipeService.CalculateFinalGravity(recipe);
        _recipeService.CalculateABV(recipe);
        _recipeService.CalculateIBU(recipe);
        _recipeService.CalculateColor(recipe);


        if (TryValidateModel(recipe.Recipe))
        {
            await _recipeService.AddAsync(recipe.Recipe);
            return RedirectToAction("Recipes", "Recipe");
        }


        recipe.HopList = await _hopService.GetAllAsync();
        recipe.FermentableList = await _fermentableService.GetAllAsync();
        recipe.YeastList = await _yeastService.GetAllAsync();

        TempData["Recipe"] = JsonSerializer.Serialize(recipe);
        return View(recipe);
    }

    public async Task<IActionResult> AddHops()
    {
        var recipeComponent = JsonSerializer.Deserialize<RecipeDetailsDto>(TempData["Recipe"].ToString());
        _recipeVm = recipeComponent;


        recipeComponent.HopList = await _hopService.GetAllAsync();
        recipeComponent.YeastList = await _yeastService.GetAllAsync();
        recipeComponent.FermentableList = await _fermentableService.GetAllAsync();

        recipeComponent.Recipe.Hops = new List<HopAddition>();

        TempData["Recipe"] = JsonSerializer.Serialize(recipeComponent);

        return View(_recipeVm);
    }

    [HttpPost]
    public async Task<IActionResult> AddHops(RecipeDetailsDto recipeVm)
    {
        var recipe = JsonSerializer.Deserialize<RecipeDetailsDto>(TempData["Recipe"].ToString());

        recipe.Recipe.Hops = new List<HopAddition>();

        if (recipeVm.HopId is not null)
        {
            for (int i = 0; i < recipeVm.HopId.Length; i++)
            {
                // Get Hop by Id from _hopService
                HopResponse hopResponse = await _hopService.GetByIdAsync(recipeVm.HopId[i]);
                Hop hop = hopResponse.ToHop();


                // Create a new HopAddition
                HopAddition hopAddition = new HopAddition
                {
                    Hop = hop,
                    Use = recipeVm.HopAdditions[i].Use,
                    BoilTime = recipeVm.HopAdditions[i].BoilTime,
                    DryHopDays = recipeVm.HopAdditions[i].DryHopDays,
                    Form = recipeVm.HopAdditions[i].Form,
                    Amount = recipeVm.HopAdditions[i].Amount
                };

                recipe.Recipe.Hops.Add(hopAddition);
            }
        }

        TempData["Recipe"] = JsonSerializer.Serialize(recipe);
        return RedirectToAction("AddRecipe");
    }

    public async Task<IActionResult> EditRecipe(int id)
    {
        ViewBag.YeastList = await _yeastService.GetAllAsync();

        RecipeResponse? recipeResponse = await _recipeService.GetByIdAsync(id);

        RecipeUpdateRequest? recipe = recipeResponse.ToRecipeUpdateRequest();


        TempData["RecipeUpdateRequest"] = JsonSerializer.Serialize(recipe);

        return View(recipe);
    }

    [HttpPost]
    public async Task<IActionResult> EditRecipe(RecipeUpdateRequest recipe)
    {
        RecipeResponse response = await _recipeService.GetByIdAsync(recipe.Id);

        RecipeUpdateRequest recipeUpdateRequest = response.ToRecipeUpdateRequest();

        recipeUpdateRequest.Name = recipe.Name;
        recipeUpdateRequest.Style = recipe.Style;
        recipeUpdateRequest.YeastAmount = recipe.YeastAmount;
        recipeUpdateRequest.YeastViability = recipe.YeastViability;
        recipeUpdateRequest.MashTemp = recipe.MashTemp;
        recipeUpdateRequest.WaterRatio = recipe.WaterRatio;

        await _recipeService.UpdateAsync(recipeUpdateRequest);


        return RedirectToAction("Recipes", "Recipe");
    }

    [HttpGet]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        RecipeResponse recipe = await _recipeService.GetByIdAsync(id);

        return View(recipe);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRecipe(RecipeResponse recipe)
    {
        bool succeeded = await _recipeService.DeleteAsync(recipe.Id);

        if (!succeeded)
        {
            return View(recipe);
        }

        return RedirectToAction("Recipes", "Recipe");
    }

    [AllowAnonymous]
    public async Task<IActionResult> ViewRecipe(int id)
    {
        RecipeResponse recipe = await _recipeService.GetByIdAsync(id);
        return View(recipe);
    }
}