using System.ComponentModel.DataAnnotations;
using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.Enums;

namespace HomebrewDesigner.Core.DTO;

/// <summary>
/// Dto for updating a recipe.
/// </summary>
public class RecipeUpdateRequest
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public StyleEnum Style { get; set; }
    
    [Required]
    [Range(.990, 1.200, ErrorMessage = "Original Gravity must be between .990 and 1.200")]
    public double? OriginalGravity { get; set; }
    
    [Required]
    [Range(.990, 1.200, ErrorMessage = "Original Gravity must be between .990 and 1.200")]
    public double FinalGravity { get; set; }
    
    [Required]
    [Range(.1, 100, ErrorMessage = "IBU must be between .1 and 100")]
    public double IBU { get; set; }
    
    [Required]
    [Range(.1, 20, ErrorMessage = "ABV must be between .1 and 20")]
    public double ABV { get; set; }
    
    public List<HopAddition> Hops { get; set; }
    
    public Yeast Yeast { get; set; }
   
    public double? YeastAmount { get; set; }
   
    public double? YeastViability { get; set; }
   
    public int? MashTemp { get; set; }
   
    public List<FermentablePair> Maltbill { get; set; }
   
    public double? WaterRatio { get; set; }
   
    public double? AmountOfWater { get; set; }
    public double Color { get; set; }
    
    public int YeastId { get; set; }
    
    public class FermentablePair
    {
        public int Id { get; set; }
        
        public  int FermentableId { get; set; }
        
        public  int RecipeId { get; set; }
        public Fermentables Fermentable { get; set; }
        
        public double? Weight { get; set; }
    }

    /// <summary>
    /// Converts RecipeUpdateRequest object to Recipe object.
    /// </summary>
    /// <returns>Returns newly created recipe object</returns>
    public Recipe ToRecipe()
    {
        return new Recipe()
        {
            Id = Id,
            Name = Name,
            Style = Style.ToString(),
            OriginalGravity = OriginalGravity,
            FinalGravity = FinalGravity,
            IBU = IBU,
            ABV = ABV,
            HopAdditions = Hops,
            Yeast = Yeast,
            YeastAmount = YeastAmount,
            YeastViability = YeastViability,
            MashTemp = MashTemp,
            Maltbill = Maltbill.Select(pair => new Recipe.FermentablePair()
            {
                RecipeId = pair.RecipeId,
                FermentableId = pair.FermentableId,
                Fermentable = pair.Fermentable,
                Weight = pair.Weight
            }).ToList(),
            WaterRatio = WaterRatio,
            AmountOfWater = AmountOfWater,
            Color = Color,
            YeastId = YeastId
        };
    }
}