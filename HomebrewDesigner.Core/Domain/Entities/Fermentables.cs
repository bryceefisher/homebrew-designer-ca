namespace HomebrewDesigner.Core.Domain.Entities;

public class Fermentables
{
    public int Id { get; set;  }
    
    public string Name { get; set;  }
    
    public string Type { get; set;  }
    
    public string Origin { get; set;  }
    
    public double Color { get; set;  }
    
    public double PotentialGravity { get; set;  }
    
    public double MaxInBatch { get; set;  }
}