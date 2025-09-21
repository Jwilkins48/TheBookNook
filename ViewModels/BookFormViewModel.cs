using System.ComponentModel.DataAnnotations;

namespace TheBooksNook.ViewModels;

public class BookFormViewModel
{
    [Key]
    public int? Id { get; set; }

    [Required(ErrorMessage = "Title Required.")]
    [MinLength(2, ErrorMessage = "Title must have at least 2 characters.")]
    public string BookTitle { get; set; } = "";

    [Required(ErrorMessage = "Author Required.")]
    [MinLength(2, ErrorMessage = "Author must have at least 2 characters.")]
    public string Author { get; set; } = "";

    [Required(ErrorMessage = "Genre Required.")]
    [MinLength(2, ErrorMessage = "Genre must have at least 2 characters.")]
    public string Genre { get; set; } = "";

    [Required(ErrorMessage = "Year Published Required.")]
    [Range(300, 2025, ErrorMessage = "Year range must be between 300 and 2025")]
    public int PublishedYear { get; set; }

    [Required(ErrorMessage = "Description Required.")]
    [MinLength(2, ErrorMessage = "Description must have at least 2 characters.")]
    public string Description { get; set; } = "";
}
