using System.ComponentModel.DataAnnotations;
using HomebrewDesigner.Core.Domain.Entities;

namespace HomebrewDesigner.Core.DTO;

/// <summary>
/// Acts as a DTO representing a Hop object to update.
/// </summary>
public class HopUpdateRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Hop name cannot be blank.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Alpha Acid cannot be blank.")]
    [Range(0.1, 30, ErrorMessage = "Alpha Acid must be between 0.1 and 30.0")]
    public double AlphaAcid { get; set; }

    /// <summary>
    /// Converts HopUpdateRequest object to Hop object.
    /// </summary>
    /// <returns>Returns newly created Hop object.</returns>
    public Hop ToHop()
    {
        return new Hop()
        {
            Id = Id,
            Name = Name,
            AlphaAcid = AlphaAcid
        };
    }
}