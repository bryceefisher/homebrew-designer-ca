using System.ComponentModel.DataAnnotations;

namespace HomebrewDesigner.ViewModels;

public class LoginVM
{
    [Required(ErrorMessage = "User name is required")]
    public string? UserName { get; set; } 
    
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    
    [Required]
    public bool RememberMe { get; set; }
}