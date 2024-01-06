using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.Enums;

namespace HomebrewDesigner.Core.DTO;

/// <summary>
/// DTO class that represents a return type for Fermentables methods
/// </summary>
public class FermentableResponse
{
    public int Id { get; set;  }
    
    public string Name { get; set;  }
    
    public string Type { get; set;  }
    
    public string Origin { get; set;  }
    
    public double Color { get; set;  }
    
    public double PotentialGravity { get; set;  }
    
    public double MaxInBatch { get; set;  }
    
    
    public override string ToString()
    {
        return $"GrainId: {Id} - Fermentables Name: {Name} - Color: {Color} - Potential Gravity: {PotentialGravity}";
    }
    
    public FermentableUpdateRequest ToGrainUpdateRequest()
    {
        return new FermentableUpdateRequest()
        {
            Id = Id,
            Name = Name,
            Type = Enum.Parse<FermentableTypeEnum>(Type),
            Origin = Enum.Parse<FermentableOriginEnum>(Origin),
            Color = Color,
            PotentialGravity = PotentialGravity,
            MaxInBatch = MaxInBatch
        };
    }
    
    public Fermentables ToFermentables()
    {
        return new Fermentables()
        {
            Id = Id,
            Name = Name,
            Type = Type,
            Origin = Origin,
            Color = Color,
            PotentialGravity = PotentialGravity,
            MaxInBatch = MaxInBatch
        };
    }
    
}

public static class FermentableExtensions
{
    /// <summary>
    /// Converts Fermentables object to FermentableResponse object.
    /// </summary>
    /// <param name="fermentables"></param>
    /// <returns>Returns the converted FermentableResponse object.</returns>
    public static FermentableResponse ToFermentableResponse(this Fermentables fermentables)
    {
        return new FermentableResponse()
        {
            Id = fermentables.Id,
            Name = fermentables.Name,
            Type = fermentables.Type,
            Origin = fermentables.Origin,
            Color = fermentables.Color,
            PotentialGravity = fermentables.PotentialGravity,
            MaxInBatch = fermentables.MaxInBatch
        };
    }
}