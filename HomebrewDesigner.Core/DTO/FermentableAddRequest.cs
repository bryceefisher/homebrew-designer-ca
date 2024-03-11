using System.ComponentModel.DataAnnotations;
using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HomebrewDesigner.Core.DTO;
/// <summary>
/// Acts as a DTO for inserting a new Fermentables
/// </summary>
public class FermentableAddRequest
{
    public int Id { get; set; }
    
    [Remote(action: "UniqueEntityName", controller: "Ingredient", ErrorMessage = "Fermentable name already exists")]
    [Required(ErrorMessage = "Fermentable Name already exists")]
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
    public Fermentables ToFermentable()
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