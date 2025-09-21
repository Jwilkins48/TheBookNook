using System.ComponentModel.DataAnnotations;

namespace TheBooksNook.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Store user books and comments
    public List<Book> Books { get; set; } = [];
    public List<Comment> Comments { get; set; } = [];
}
