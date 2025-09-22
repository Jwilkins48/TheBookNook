using System.ComponentModel.DataAnnotations;

namespace TheBooksNook.ViewModels;

public class CommentFormViewModel
{
    public int BookId { get; set; }

    [Required(ErrorMessage = "Please Enter a Comment")]
    [MinLength(2, ErrorMessage = "Comment must have at least 2 characters.")]
    public string CommentContext { get; set; } = "";
}
