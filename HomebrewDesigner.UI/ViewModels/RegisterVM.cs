using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace HomebrewDesigner.ViewModels;

public class RegisterVM
{
    [Remote(action: "UniqueEmail", controller: "Account", ErrorMessage = "Email is already in use")]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Person name is required")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "Person name is required")]
    public string LastName { get; set; }
    
    [Remote(action: "UniqueUserName", controller: "Account", ErrorMessage = "UserName is already in use")]
    [Required(ErrorMessage = "UserName name is required")]
    public string UserName { get; set; }
    
    
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Confirm password is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
    public string ConfirmPassword { get; set; }
    
    public bool IsAdmin { get; set; }
    
}