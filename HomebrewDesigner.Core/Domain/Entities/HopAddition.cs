namespace HomebrewDesigner.Core.Domain.Entities;

public class HopAddition
{
    public int Id { get; set; }
    public Hop? Hop { get; set; }
    
    public string Use { get; set; }
    
    public int? BoilTime { get; set; }
    
    public int? DryHopDays { get; set; }
    
    public string Form { get; set; }
    
    public double? Amount { get; set; }
    
    public int RecipeId { get; set; }
    
    public int HopId { get; set; }
}