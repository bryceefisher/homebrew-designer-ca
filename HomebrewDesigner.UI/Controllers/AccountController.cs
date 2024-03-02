using HomebrewDesigner.Core.Domain.Identity;
using HomebrewDesigner.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomebrewDesigner.Controllers;

[Route("[controller]/[action]")]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    #region Register
    
    
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM registerVm)
    {
        registerVm.IsAdmin = false;
        
        // Validate the model state
        if (ModelState.IsValid)
        {
            // Create a new ApplicationUser object
            ApplicationUser user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(), //  Generate a new GUID for the user ID
                FirstName = registerVm.FirstName, // Set the first name from the DTO
                LastName = registerVm.LastName, // Set the last name from the DTO
                UserName = registerVm.UserName, // Set the username to the email from the DTO
                Email = registerVm.Email,// Set the email from the DTO
                
            };

            // Attempt to create the user with the provided password
            IdentityResult result = await _userManager.CreateAsync(user, registerVm.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user,
                    false);
                return RedirectToAction("Home", "Home");
            }
            
            // Add errors to ModelState and return to the registration view
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("Register", error.Description);
            }

            return View(registerVm);
        }

        // If ModelState is not valid, return to the view with validation errors
        return View(registerVm);
    }
    
    #endregion

    #region Login/Logout

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVm, string? ReturnUrl)
    {
        // Validate the model state
        if (ModelState.IsValid)
        {
            // Attempt to sign in the user with the provided credentials
            var result = await _signInManager.PasswordSignInAsync(loginVm.UserName, loginVm.Password, loginVm.RememberMe, false);
            
            // Check if the sign in was successful
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    // Redirect to the ReturnUrl
                    return LocalRedirect(ReturnUrl);
                }
                // Redirect to the Person Index page
                return RedirectToAction("Home", "Home");
            }

            // Add error to ModelState and return to the login view
            ModelState.AddModelError("Login", "Invalid username or password");
            return View(loginVm);
        }

        // If ModelState is not valid, return to the view with validation errors
        return View(loginVm);
    }
    
    public async Task<IActionResult> Logout()
    {
        // Sign out the user
        await _signInManager.SignOutAsync();

        // Redirect to the login page
        return RedirectToAction("Login", "Account");
    }

    #endregion
    
    
    
    [AllowAnonymous]
    //async method for checking if the email is unique
    public async Task<IActionResult> UniqueEmail(string email)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(email);

        if (user is not null)
        {
            return Json(false); //already exists
        }
        else
        {
            return Json(true); //valid
        }
    }
    
    [AllowAnonymous]
    //async method for checking if the email is unique
    public async Task<IActionResult> UniqueUserName(string username)
    {
        ApplicationUser? user = await _userManager.FindByNameAsync(username);

        if (user is not null)
        {
            return Json(false); //already exists
        }
        else
        {
            return Json(true); //valid
        }
    }
    
}