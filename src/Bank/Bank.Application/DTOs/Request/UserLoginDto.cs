using System.ComponentModel.DataAnnotations;

namespace Bank.Bank.Application.DTOs.Request;

public class UserLoginDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    [MaxLength(30, ErrorMessage = "Password must be between 8 and 30 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\d])(?=.*[@%#$*^{}&]).+$"
        ,ErrorMessage = "Password does no meet requirements")]
    public string Password { get; set; } = string.Empty;    
}