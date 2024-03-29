using System.ComponentModel.DataAnnotations;
using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HomebrewDesigner.Core.DTO;

/// <summary>
/// Acts as a DTO for inserting a new Yeast
/// </summary>
public class YeastAddRequest
{
    public int Id { get; set;  }
    
    [Remote(action: "UniqueEntityName", controller: "Ingredient", ErrorMessage = "Yeast name already exists")]
    [Required(ErrorMessage = "Yeast Name already exists")]
    public string Name { get; set;  }
    
    [Required]
    public string Lab { get; set;  }
    
    [Required]
    public string Code { get; set;  }
    
    [Required]
    public YeastTypeEnum Type { get; set;  }
    
    [Required]
    public YeastFormEnum Form { get; set;  }
    
    [Required]
    public YeastFlocEnum Flocculation { get; set;  }
    
    
    /// <summary>
    /// Creates a new Yeast object from the current YeastAddRequest object.
    /// </summary>
    /// <returns>Newly created Yeast objects.</returns>
    public Yeast ToYeast()
    {
        return new Yeast()
        {
            Id = Id,
            Name = Name,
            Lab = Lab,
            Code = Code,
            Type = Type.ToString(),
            Form = Form.ToString(),
            Flocculation = Flocculation.ToString()
        };
    }
}