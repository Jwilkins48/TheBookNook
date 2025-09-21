using System.ComponentModel.DataAnnotations;

namespace TheBooksNook.ViewModels;

public class LoginFormViewModel
{
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Please enter email")]
    [EmailAddress(ErrorMessage = "Please enter valid email")]
    public string Email { get; set; } = "";

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Please enter password")]
    public string Password { get; set; } = "";
    public string? Error { get; set; }
}
