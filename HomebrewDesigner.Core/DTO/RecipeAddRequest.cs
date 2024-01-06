using HomebrewDesigner.Core.Domain.Entities;

namespace HomebrewDesigner.Core.DTO;

/// <summary>
/// Acts as a DTO representing a Recipe object to add.
/// </summary>
[Serializable]
public class RecipeAddRequest
{
    public int Id { get; set; }
    
 
    public string Name { get; set; }
    
    public string Style { get; set; }
    
  
   
    public double? OriginalGravity { get; set; }
    
   
    public double FinalGravity { get; set; }
    
  
    public double IBU { get; set; }
    
    
    public double ABV { get; set; }
    
    public List<HopAddition>? Hops { get; set; }
    
    public Yeast? Yeast { get; set; }
   
    public double? YeastAmount { get; set; }
   
    public double? YeastViability { get; set; }
   
  
    public int? MashTemp { get; set; }
   
    public List<FermentablePair>? MaltBill { get; set; }
    
   
    public double? WaterRatio { get; set; }
   
  
 
    public double AmountOfWater { get; set; }
    
    public double Color { get; set; }
    
    public int YeastId { get; set; }
    
    public class FermentablePair
    {
        public int Id { get; set; }

        public  int RecipeId { get; set; }

        public int FermentableId { get; set; }
        
        public Fermentables Fermentable { get; set; }
        public double? Weight { get; set; }
    }
    
    /// <summary>
    /// Returns a Recipe object from RecipeAddRequest object.
    /// </summary>
    /// <returns>Newly created recipe object.</returns>
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
            Maltbill = MaltBill.Select(pair => new Recipe.FermentablePair
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