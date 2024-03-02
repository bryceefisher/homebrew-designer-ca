using HomebrewDesigner.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace HomebrewDesigner.ViewModels;

public class UserVm
{
    public IEnumerable<ApplicationUser> Users { get; set; }

    public IEnumerable<IdentityRole> Roles { get; set; }
}