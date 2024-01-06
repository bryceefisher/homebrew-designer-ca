using System.ComponentModel.DataAnnotations;

namespace HomebrewDesigner.Core.Domain.Entities;

public class Recipe
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Style { get; set; }

    public double? OriginalGravity { get; set; }

    public double FinalGravity { get; set; }

    public double IBU { get; set; }

    public double ABV { get; set; }

    public List<HopAddition> HopAdditions { get; set; }

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
        [Key]
        public int Id { get; set; }
        
        public int RecipeId { get; set; }
        
        public int FermentableId { get; set; }
        
        public Fermentables Fermentable { get; set; }
        
        public double? Weight { get; set; }
    }
}