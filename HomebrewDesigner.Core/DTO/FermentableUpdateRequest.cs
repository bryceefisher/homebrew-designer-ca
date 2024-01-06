using System.ComponentModel.DataAnnotations;
using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.Enums;

namespace HomebrewDesigner.Core.DTO;

public class FermentableUpdateRequest
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Fermentables name cannot be blank.")]
    public string Name { get; set; }
    
    [Required]
    public FermentableTypeEnum Type { get; set; }
    
    [Required]
    public FermentableOriginEnum Origin { get; set; }
    
    
    public double Color { get; set; }
    
    [Required]
    [Range(1.025, 1.050, ErrorMessage = "Potential Gravity must be between 1.025 and 1.050")]
    public double PotentialGravity { get; set; }
    
    [Required]
    [Range(.1, 100, ErrorMessage = "Max in Batch must be between .1 and 100")]
    public double MaxInBatch { get; set; }
    
    /// <summary>
    /// Converts the current object of FermentableAddRequest to Fermentables type object.
    /// </summary>
    /// <returns>Newly created Fermentables object.</returns>
    public Fermentables ToGrain()
    {
        return new Fermentables()
        {
            Id = Id,
            Name = Name,
            Type = Type.ToString(),
            Origin = Origin.ToString(),
            Color = Color,
            PotentialGravity = PotentialGravity,
            MaxInBatch = MaxInBatch
        };
    }
}