using System.ComponentModel.DataAnnotations;
using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.Enums;

namespace HomebrewDesigner.Core.DTO;

public class YeastUpdateRequest
{
    public int Id { get; set;  }
    
    [Required]
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