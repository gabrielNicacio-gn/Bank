using System.ComponentModel.DataAnnotations;

namespace Bank.Bank.Application.DTOs.Request;

public class UserRegister
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\d])(?=.*[@%#$*^]).+$")]
    public string Password { get; set; } = string.Empty;    
    
    [Required(ErrorMessage = "Document is required")]
    [RegularExpression(@"^[\d{3}]\.[\d{3}]\.[\d{3}]\-\d{2}$")]
    public string Document { get; set; } = string.Empty;
}