using System.ComponentModel.DataAnnotations;

namespace TheBooksNook.ViewModels;

public class RegisterFormViewModel
{
    [Required(ErrorMessage = "Username Required")]
    [MinLength(2, ErrorMessage = "Username must have at least 2 characters")]
    public string Username { get; set; } = "";

    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Email Required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email")]
    public string Email { get; set; } = "";

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password Required")]
    [MinLength(8, ErrorMessage = "Password must have at least 8 characters")]
    public string Password { get; set; } = "";

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Please confirm your password")]
    [Compare("Password", ErrorMessage = "Passwords must match")]
    public string ConfirmPassword { get; set; } = "";
}
