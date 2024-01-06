using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.DTO;
using HomebrewDesigner.Core.ServiceContracts;

namespace HomebrewDesigner.ViewModels;

public class RecipeVM
{
    private  IHopService _hopService;
    private  IFermentableService _fermentableService;
    private  IYeastService _yeastService;

   
    public RecipeVM(IHopService hopService, IFermentableService fermentableService, IYeastService yeastService)
    {
        _hopService = hopService;
        _fermentableService = fermentableService;
        _yeastService = yeastService;
    }
    
    public RecipeVM()
    {
        
    }
    
    public RecipeAddRequest Recipe { get; set; } 
    
    public int YeastId { get; set; }
    public int[]? HopId { get; set; }

    public int[]? FermentableId { get; set; }
    
    
    public double[]? FermentableWeight { get; set; }

    public List<HopResponse>? HopList { get; set; }
    public List<FermentableResponse>? FermentableList { get; set; } 
    public List<YeastResponse>? YeastList { get; set; } 
    public HopAddition[]? HopAdditions { get; set; }

    
}

