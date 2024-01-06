using HomebrewDesigner.Core.Domain.Entities;
using HomebrewDesigner.Core.Enums;

namespace HomebrewDesigner.Core.DTO;

/// <summary>
/// Dto class that represents a return type for Yeast methods
/// </summary>
public class YeastResponse
{
    public int Id { get; set;  }
    
    public string Name { get; set;  }
    
    public string Lab { get; set;  }
    
    public string Code { get; set;  }
    
    public string Type { get; set;  }
    
    public string Form { get; set;  }
    
    public string Flocculation { get; set;  }


    public override string ToString()
    {
        return $"YeastId: {Id} - Yeast Name: {Name} - Lab: {Lab} - Code: {Code} - Type: {Type} - Form: {Form} Flocculation: {Flocculation}\n";
    }
    
    public YeastUpdateRequest ToYeastUpdateRequest()
    {
        return new YeastUpdateRequest()
        {
            Id = Id,
            Name = Name,
            Lab = Lab,
            Code = Code,
            Type = Enum.Parse<YeastTypeEnum>(Type),
            Form = Enum.Parse<YeastFormEnum>(Form),
            Flocculation = Enum.Parse<YeastFlocEnum>(Flocculation)    
        };
    }
    
    public Yeast ToYeast()
    {
        return new Yeast()
        {
            Id = Id,
            Name = Name,
            Lab = Lab,
            Code = Code,
            Type = Type,
            Form = Form,
            Flocculation = Flocculation  
        };
    }
}

public static class YeastExtensions
{
    /// <summary>
    /// Converts Yeast object to YeastResponse object.
    /// </summary>
    /// <param name="yeast"></param>
    /// <returns>Returns the converted YeastResponse object.</returns>
    public static YeastResponse ToYeastResponse(this Yeast yeast)
    {
        return new YeastResponse()
        {
            Id = yeast.Id,
            Name = yeast.Name,
            Lab = yeast.Lab,
            Code = yeast.Code,
            Type = yeast.Type,
            Form = yeast.Form,
            Flocculation = yeast.Flocculation
        };
    }
}