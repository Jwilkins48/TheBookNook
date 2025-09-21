using System.ComponentModel.DataAnnotations;

namespace TheBooksNook.Models;

public class Book
{
    [Key]
    public int Id { get; set; }
    public string BookTitle { get; set; } = "";
    public string Author { get; set; } = "";
    public string Genre { get; set; } = "";
    public int PublishedYear { get; set; }
    public string BookImageSrc { get; set; } = "";
    public string Description { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Foreing Key
    public int UserId { get; set; }
    public User? User { get; set; }

    // Store Comments
    public List<Comment> Comments { get; set; } = [];
}
