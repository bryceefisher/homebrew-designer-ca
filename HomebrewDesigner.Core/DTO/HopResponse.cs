

using HomebrewDesigner.Core.Domain.Entities;

namespace HomebrewDesigner.Core.DTO;

/// <summary>
/// DTO class that represents a return type for Hop methods
/// </summary>
public class HopResponse
{
    public int Id { get; set;  }
    
    public string Name { get; set;  }
    
    public double AlphaAcid { get; set;  }

    public override string ToString()
    {
        return $"HopId: {Id} - Hop Name: {Name} - Alpha Acid%: {AlphaAcid}";
    }
    
    public HopUpdateRequest ToHopUpdateRequest()
    {
        return new HopUpdateRequest()
        {
            Id = Id,
            Name = Name,
            AlphaAcid = AlphaAcid
        };
    }
    
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


public static class HopExtensions
{
    /// <summary>
    /// Converts Hop object to HopResponse object.
    /// </summary>
    /// <param name="hop"></param>
    /// <returns>Returns the converted HopResponse object.</returns>
    public static HopResponse ToHopResponse(this Hop hop)
    {
        return new HopResponse()
        {
            Id = hop.Id,
            Name = hop.Name,
            AlphaAcid = hop.AlphaAcid
        };
    }
}