using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HomebrewDesigner.Core.Domain.Identity;

public class ApplicationUser : IdentityUser<string>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    [NotMapped]
    public IList<string> RoleNames { get; set; }
}