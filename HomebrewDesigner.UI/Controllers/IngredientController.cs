using System.Collections;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.Enums;
using HomebrewDesigner.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace HomebrewDesigner.Controllers;

[Route("[controller]/[action]")]
public class IngredientController : Controller
{
    private readonly ILogger<IngredientController> _logger;
    private readonly IHopService _hopService;
    private readonly IFermentableService _fermentableService;
    private readonly IYeastService _yeastService;

    public IngredientController(ILogger<IngredientController> logger, IHopService hopService,
        IFermentableService fermentableService, IYeastService yeastService)
    {
        _logger = logger;
        _hopService = hopService;
        _fermentableService = fermentableService;
        _yeastService = yeastService;
    }

    // GET
    public async Task<IActionResult> Hops(string? searchBy, string? searchString)
    {
        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            { nameof(HopResponse.Name), "Name" },
            { nameof(HopResponse.AlphaAcid), "Alpha Acid" }
        };

        IEnumerable<HopResponse> hops = await _hopService.GetFilteredHopsAsync(searchBy, searchString);
        
        hops = hops.OrderBy(h => h.Name);

        if (hops.Any())
        {
            return View(hops);
        }

        return View();
    }

    // GET
    public async Task<IActionResult> Yeast(string? searchBy, string? searchString)
    {
        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            { nameof(YeastResponse.Name), "Name" },
            { nameof(YeastResponse.Lab), "Lab" },
            { nameof(YeastResponse.Code), "Product Code" },
            { nameof(YeastResponse.Type), "Type" },
            { nameof(YeastResponse.Form), "Form" },
            { nameof(YeastResponse.Flocculation), "Flocculation" }
        };

        IEnumerable<YeastResponse> yeast = await _yeastService.GetFilteredYeastAsync(searchBy, searchString);
        
        yeast = yeast.OrderBy(y => y.Name);
        

        if (yeast.Any())
        {
            return View(yeast);
        }

        return View();
    }

    // GET
    public async Task<IActionResult> Fermentables(string? searchBy, string? searchString)
    {
        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            { nameof(FermentableResponse.Name), "Name" },
            { nameof(FermentableResponse.Type), "Type" },
            { nameof(FermentableResponse.Origin), "Origin" },
            { nameof(FermentableResponse.Color), "Color" },
            { nameof(FermentableResponse.PotentialGravity), "Potential Gravity" },
            { nameof(FermentableResponse.MaxInBatch), "Max in Batch %" }
        };

        IEnumerable<FermentableResponse> fermentable =
            await _fermentableService.GetFilteredFermentableAsync(searchBy, searchString);
        
        IEnumerable<FermentableResponse> orderedFermentable = fermentable.OrderBy(f => f.Name);

        if (orderedFermentable.Any())
        {
            return View(orderedFermentable);
        }

        return View();
    }

    public async Task<IActionResult> EditHop(int Id)
    {
        HopResponse hopResponse = await _hopService.GetHopByIdAsync(Id);
        
        HopUpdateRequest hop = hopResponse.ToHopUpdateRequest();
        
        return View(hop);
    }

    [HttpPost]
    public async Task<IActionResult> EditHop(HopUpdateRequest hop)
    {
        if (ModelState.IsValid)
        {
           await  _hopService.UpdateHopAsync(hop);
        }

        return RedirectToAction("Hops", "Ingredient");
    }

    public IActionResult AddHop()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddHop(HopAddRequest hop)
    {
       await  _hopService.AddHopAsync(hop);

        return RedirectToAction("Hops", "Ingredient");
    }

    public async Task<IActionResult> EditYeast(int Id)
    {
        YeastResponse? yeastResponse = await _yeastService.GetYeastByIdAsync(Id);
        YeastUpdateRequest yeast = yeastResponse.ToYeastUpdateRequest();

        ViewBag.Types = Enum.GetNames(typeof(YeastTypeEnum));
        ViewBag.Forms = Enum.GetNames(typeof(YeastFormEnum));
        ViewBag.Flocs = Enum.GetNames(typeof(YeastFlocEnum));

        return View(yeast);
    }

    [HttpPost]
    public async Task<IActionResult> EditYeast(YeastUpdateRequest yeast)
    {
        if (ModelState.IsValid)
        {
            await _yeastService.UpdateYeastAsync(yeast);
            return RedirectToAction("Yeast", "Ingredient");
        }
        else
        {
            ViewBag.Types = Enum.GetNames(typeof(YeastTypeEnum));
            ViewBag.Forms = Enum.GetNames(typeof(YeastFormEnum));
            ViewBag.Flocs = Enum.GetNames(typeof(YeastFlocEnum));
            return View(yeast);
        }
    }

    public IActionResult AddYeast()
    {
        ViewBag.Types = Enum.GetNames(typeof(YeastTypeEnum));
        ViewBag.Forms = Enum.GetNames(typeof(YeastFormEnum));
        ViewBag.Flocs = Enum.GetNames(typeof(YeastFlocEnum));
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddYeast(YeastAddRequest yeast)
    {
        if (ModelState.IsValid)
        {
           await  _yeastService.AddYeastAsync(yeast);
        }

        return RedirectToAction("Yeast", "Ingredient");
    }

    public async Task<IActionResult> EditFermentable(int Id)
    {
        FermentableResponse? grain = await _fermentableService.GetFermentableByIdAsync(Id);
        FermentableUpdateRequest fermentable = grain.ToGrainUpdateRequest();

        ViewBag.Origins = Enum.GetNames(typeof(FermentableOriginEnum));
        ViewBag.Types = Enum.GetNames(typeof(FermentableTypeEnum));

        return View(fermentable);
    }

    [HttpPost]
    public async Task<IActionResult> EditFermentable(FermentableUpdateRequest fermentable)
    {
        if (ModelState.IsValid)
        {
            await _fermentableService.UpdateFermentableAsync(fermentable);
            return RedirectToAction("Fermentables", "Ingredient");
        }
        else
        {
            ViewBag.Origins = Enum.GetNames(typeof(FermentableOriginEnum));
            ViewBag.Types = Enum.GetNames(typeof(FermentableTypeEnum));
            return View(fermentable);
        }
    }

    public IActionResult AddFermentable()
    {
        ViewBag.Types = Enum.GetNames(typeof(FermentableTypeEnum));
        ViewBag.Origins = Enum.GetNames(typeof(FermentableOriginEnum));
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddFermentable(FermentableAddRequest fermentable)
    {
        if (ModelState.IsValid)
        {
            await _fermentableService.AddFermentableAsync(fermentable);
            return RedirectToAction("Fermentables", "Ingredient");
        }

        ViewBag.Types = Enum.GetNames(typeof(FermentableTypeEnum));
        ViewBag.Origins = Enum.GetNames(typeof(FermentableOriginEnum));
        
        return View();
    }
}