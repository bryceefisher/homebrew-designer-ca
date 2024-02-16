using HomebrewDesigner.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace HomebrewDesigner.Infrastructure.DbContext;

public class SeedData
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    
    public SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedUsers()
    {
        if (!_userManager.Users.Any())
        {
            const string secretPassword = "Secret!123";

            ApplicationUser marv = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(), UserName = "Marv", FirstName = "Marv", LastName = "Fisher",
                Email = "marv@gmail.com"
            };
            IdentityResult result = await _userManager.CreateAsync(marv, secretPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(marv, "User");
            }

            ApplicationUser bryceFisher = new ApplicationUser
            {
                Id=Guid.NewGuid().ToString(), UserName = "Bryce", FirstName = "Bryce", LastName = "Fisher",
                Email = "bryce@gmail.com"
            };
            result = await _userManager.CreateAsync(bryceFisher, secretPassword);
            if (result.Succeeded)
            {
                IEnumerable<string> roles = new List<string>()
                {
                    "Admin",
                    "User"
                };
                
                await _userManager.AddToRolesAsync(bryceFisher, roles);
            }

            ApplicationUser brianBird = new ApplicationUser
            {
                Id=Guid.NewGuid().ToString(), UserName = "Bird", FirstName = "Brian", LastName = "Bird",
                Email = "bird@gmail.com"
            };
            result =  await _userManager.CreateAsync(brianBird, secretPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(brianBird, "User");  
            }
        }
    }
    
    public async Task SeedRoles()
    {
        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await _roleManager.RoleExistsAsync("User"))
        {
            await _roleManager.CreateAsync(new IdentityRole("User"));
        }
        
        if (!await _roleManager.RoleExistsAsync("Guest"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Guest"));
        }
    }
}