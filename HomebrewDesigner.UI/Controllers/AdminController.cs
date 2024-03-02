using HomebrewDesigner.Core.Domain.Identity;
using HomebrewDesigner.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomebrewDesigner.Controllers;

[Route("[controller]/[action]")]
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        List<ApplicationUser> users = _userManager.Users.ToList();

        foreach (var user in users)
        {
            user.RoleNames = await _userManager.GetRolesAsync(user);
        }

        UserVm userVm = new()
        {
            Users = users,
            Roles = _roleManager.Roles
        };
        
        if (TempData["ErrorMessage"] != null)
        {
            ModelState.AddModelError(string.Empty, TempData["ErrorMessage"].ToString());
        }

        return View(userVm);
    }

    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(RegisterVM registerVm)
    {
        // Validate the model state
        if (ModelState.IsValid)
        {
            // Create a new ApplicationUser object
            ApplicationUser user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = registerVm.FirstName,
                LastName = registerVm.LastName,
                UserName = registerVm.UserName,
                Email = registerVm.Email,
            };

            // Attempt to create the user with the provided password
            IdentityResult result = await _userManager.CreateAsync(user, registerVm.Password);

            if (registerVm.IsAdmin)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            // Add errors to ModelState and return to the registration view
            foreach (IdentityError error in result.Errors)
            {
                TempData["ErrorMessage"] = "Role not found, or a user is assigned to role.";
            }

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }
        }
        // If ModelState is not valid, return to the view with validation errors
        return View(registerVm);
    }

    public async Task<IActionResult> Delete(string id)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(id);

        var result = await _userManager.DeleteAsync(user);

        foreach (IdentityError error in result.Errors)
        {
            ModelState.AddModelError("Register", error.Description);
        }

        return RedirectToAction("Index", "Admin");
    }

    public async Task<IActionResult> AddToAdmin(string id)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(id);


        if (user is not null && await _userManager.IsInRoleAsync(user, "Admin") == false)
        {
            IdentityResult result = await _userManager.AddToRoleAsync(user, "Admin");

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("AddToAdmin", error.Description);
            }

            return RedirectToAction("Index", "Admin");
        }

        
        TempData["ErrorMessage"] = "User is already an admin, or user is null.";
        return RedirectToAction("Index", "Admin");

    }

    public async Task<IActionResult> RemoveFromAdmin(string id)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(id);

        if (user is not null && await _userManager.IsInRoleAsync(user, "Admin"))
        {
            IdentityResult result = await _userManager.RemoveFromRoleAsync(user, "Admin");

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("Register", error.Description);
            }

            return RedirectToAction("Index", "Admin");
        }
        
        TempData["ErrorMessage"] = "User is not an admin, or user is null.";
        return RedirectToAction("Index", "Admin");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(id);

        if (user is not null)
        {
            return View(user);
        }

        
        TempData["ErrorMessage"] = "User not found.";
        return RedirectToAction("Index", "Admin");
    }


    [HttpPost]
    public async Task<IActionResult> Edit(string id, ApplicationUser request)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(id);

        if (user is not null)
        {
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.UserName = request.UserName;

            IdentityResult result = await _userManager.UpdateAsync(user);

            return RedirectToAction("Index", "Admin");
        }
        
        TempData["ErrorMessage"] = "Error editing user.";
        return RedirectToAction("Index", "Admin");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromUserRole(string id)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(id);

        if (user is not null)
        {
            IdentityResult result = await _userManager.RemoveFromRoleAsync(user, "User");
            if (result.Succeeded && await _userManager.IsInRoleAsync(user, "Guest") == false)
            {
                result = await _userManager.AddToRoleAsync(user, "Guest");
            }
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("RemoveFromUser", error.Description);
            }

            return RedirectToAction("Index", "Admin");
        }
        
        TempData["ErrorMessage"] = "User is not an admin, or user is null.";
        return RedirectToAction("Index", "Admin");
    }

    [HttpPost]
    public async Task<IActionResult> AddToUserRole(string id)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(id);

        if (user is not null)
        {
            if (await _userManager.IsInRoleAsync(user, "Guest"))
            {
                IdentityResult result = await _userManager.RemoveFromRoleAsync(user, "Guest");
            }
            
            IdentityResult userResult = await _userManager.AddToRoleAsync(user, "User");

            if (userResult.Succeeded)
            {
              return RedirectToAction("Index", "Admin");  
            }
            else
            {
                foreach (IdentityError error in userResult.Errors)
                {
                    ModelState.AddModelError("AddUser", error.Description);
                }

                return RedirectToAction("Index", "Admin");
            }
            
        }
        TempData["ErrorMessage"] = "User not found.";
        return RedirectToAction("Index", "Admin");
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteRole(string id)
    {
        IdentityRole role = await _roleManager.FindByIdAsync(id);
        bool roleOccupied = false;
        foreach (var user in await _userManager.Users.ToListAsync())
        {
            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                roleOccupied = true;
            }
            
        }
        if (role is not null && roleOccupied == false)
        {
            IdentityResult result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("DeleteRole", error.Description);
                }

                return RedirectToAction("Index", "Admin");
            }
        }

        TempData["ErrorMessage"] = "Role not found, or a user is assigned to role.";
        return RedirectToAction("Index", "Admin");
    } 

}