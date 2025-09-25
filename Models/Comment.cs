using System.ComponentModel.DataAnnotations;

namespace TheBooksNook.Models;

public class Comment
{
    [Key]
    public int Id { get; set; }
    public string CommentContext { get; set; } = "";
    public string CommentedBy { get; set; } = "";

    // Foreign Keys
    public int UserId { get; set; }
    public User? User { get; set; }

    public int BookId { get; set; }
    public Book? Book { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
