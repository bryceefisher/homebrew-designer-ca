using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.Enums;

namespace HomebrewDesigner.Core.DTO;

/// <summary>
/// DTO that is a return type for Recipe.
/// </summary>
public class RecipeResponse
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Style { get; set; }

    public double? OriginalGravity { get; set; }

    public double FinalGravity { get; set; }

    public double IBU { get; set; }

    public double ABV { get; set; }

    public List<HopAddition> Hops { get; set; }

    public Yeast Yeast { get; set; }
    
    public int YeastId { get; set; }

    public double? YeastAmount { get; set; }

    public double? YeastViability { get; set; }

    public int? MashTemp { get; set; }

    public List<FermentablePair> Maltbill { get; set; }

    public double? WaterRatio { get; set; }

    public double? AmountOfWater { get; set; }

    public double Color { get; set; }
    
    public class FermentablePair
    {
        public int Id { get; set; }
        
        public int FermentableId { get; set; }
        
        public  int RecipeId { get; set; }
        
        public Fermentables Fermentable { get; set; }
        
        public double? Weight { get; set; }
    }

    //Todo update
    public override string ToString()
    {
        string? recipeReturn = "";
        recipeReturn +=
            $"Name: {Name}, Style: {Style}, Original Gravity: {OriginalGravity}, Final Gravity: {FinalGravity}, IBU: {IBU}, ABV: {ABV}, Color: {Color}\n";

        foreach (var hopAddition in Hops)
        {
            recipeReturn += hopAddition.ToString();
        }

        return recipeReturn;
    }

    /// <summary>
    /// Converts RecipeResponse object to RecipeUpdateRequest object.
    /// </summary>
    /// <returns></returns>
    public RecipeUpdateRequest ToRecipeUpdateRequest()
    {
        return new RecipeUpdateRequest()
        {
            Id = Id,
            Name = Name,
            Style = (StyleEnum)Enum.Parse(typeof(StyleEnum), Style, true),
            OriginalGravity = OriginalGravity,
            FinalGravity = FinalGravity,
            IBU = IBU,
            ABV = ABV,
            Hops = Hops,
            Yeast = Yeast,
            YeastAmount = YeastAmount,
            YeastViability = YeastViability,
            MashTemp = MashTemp,
            Maltbill = Maltbill.Select(pair => new RecipeUpdateRequest.FermentablePair
            {
                RecipeId = pair.RecipeId,
                FermentableId = pair.FermentableId,
                Fermentable = pair.Fermentable,
                Weight = pair.Weight
            }).ToList(),
            WaterRatio = WaterRatio,
            AmountOfWater = AmountOfWater,
            Color = Color
        };
    }
    
    
}

public static class RecipeExtensions
{
    /// <summary>
    /// Converts Recipe object to RecipeResponse object.
    /// </summary>
    /// <param name="recipe"></param>
    /// <returns></returns>
    public static RecipeResponse ToRecipeResponse(this Recipe recipe)
    {
        return new RecipeResponse()
        {
            Id = recipe.Id,
            Name = recipe.Name,
            Style = recipe.Style,
            OriginalGravity = recipe.OriginalGravity,
            FinalGravity = recipe.FinalGravity,
            IBU = recipe.IBU,
            ABV = recipe.ABV,
            Hops = recipe.HopAdditions,
            Yeast = recipe.Yeast,
            YeastAmount = recipe.YeastAmount,
            YeastViability = recipe.YeastViability,
            MashTemp = recipe.MashTemp,
            Maltbill = recipe.Maltbill.Select(pair => new RecipeResponse.FermentablePair
            {
                FermentableId = pair.FermentableId,
                RecipeId = pair.RecipeId,
                Fermentable = pair.Fermentable,
                Weight = pair.Weight
            }).ToList(),
            WaterRatio = recipe.WaterRatio,
            AmountOfWater = recipe.AmountOfWater,
            Color = recipe.Color,
            YeastId = recipe.YeastId
        };
    }
}