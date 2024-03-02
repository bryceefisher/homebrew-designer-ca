using System.ComponentModel.DataAnnotations;
using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.Enums;

namespace HomebrewDesigner.Core.DTO;

public class YeastUpdateRequest
{
    public int Id { get; set;  }
    
    [Required (ErrorMessage = "Name is required.")]
    public string Name { get; set;  }
    
    [Required (ErrorMessage = "Lab is required.")]
    public string Lab { get; set;  }
    
    [Required (ErrorMessage = "Code is required.")]
    public string Code { get; set;  }
    
    [Required (ErrorMessage = "Type is required.")]
    public YeastTypeEnum Type { get; set;  }
    
    [Required (ErrorMessage = "Form is required.")]
    public YeastFormEnum Form { get; set;  }
    
    [Required (ErrorMessage = "Flocculation is required.")]
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