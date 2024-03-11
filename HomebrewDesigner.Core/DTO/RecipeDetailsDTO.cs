using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.ServiceContracts;

namespace HomebrewDesigner.Core.DTO;

public class RecipeDetailsDto
{
    private readonly IService<HopAddRequest, HopUpdateRequest, HopResponse> _hopService;
    private readonly IService<FermentableAddRequest, FermentableUpdateRequest, FermentableResponse> _fermentableService;
    private readonly IService<YeastAddRequest, YeastUpdateRequest, YeastResponse> _yeastService;


    public RecipeDetailsDto(IService<HopAddRequest, HopUpdateRequest, HopResponse> hopService,
        IService<FermentableAddRequest, FermentableUpdateRequest, FermentableResponse> fermentableService,
        IService<YeastAddRequest, YeastUpdateRequest, YeastResponse> yeastService)
    {
        _hopService = hopService;
        _fermentableService = fermentableService;
        _yeastService = yeastService;
    }

    public RecipeDetailsDto()
    {
    }

    public RecipeAddRequest? Recipe { get; set; }

    public int YeastId { get; set; }
    
    public int[]? HopId { get; set; }
    public int[]? FermentableId { get; set; }
    
    
    public double[]? FermentableWeight { get; set; }

    public List<HopResponse>? HopList { get; set; }
    public List<FermentableResponse>? FermentableList { get; set; }
    public List<YeastResponse>? YeastList { get; set; }
    
    public HopAddition[]? HopAdditions { get; set; }
}